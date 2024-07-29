
using System.Globalization;
using System;
using AssecoPraksa.Commands;
using AssecoPraksa.Controllers;
using AssecoPraksa.Database.Entities;
using AssecoPraksa.Database.Repositories;
using AssecoPraksa.Models;
using AutoMapper;
using CsvHelper;
using AssecoPraksa.Mappings;
using Microsoft.EntityFrameworkCore;
using CsvHelper.TypeConversion;
using ISO._4217;

namespace AssecoPraksa.Services
{

    public class TransactionService : ITransactionService
    {
        ITransactionRepository _repository;
        IMapper _mapper;
        ICategoryRepository _categoryRepository;
        ITransactionSplitRepository _transactionSplitRepository;
        private readonly ILogger<TransactionService> _logger;

        public TransactionService(ILogger<TransactionService> logger, ITransactionRepository repostitory, ICategoryRepository categoryRepository, ITransactionSplitRepository transactionSplitRepository, IMapper mapper)
        {
            _repository = repostitory;
            _categoryRepository = categoryRepository;
            _mapper = mapper;
            _logger = logger;
            _transactionSplitRepository = transactionSplitRepository;
        }

        public async Task<TransactionPagedList<TransactionWithSplits>> getTransactionsAsync(int page, int pageSize, SortOrder sortOrder, string? sortBy, DateTime? start = null, DateTime? end = null, string? transactionKind = null)
        {
            var transactions = await _repository.GetTransactionsAsync(page, pageSize, sortOrder, sortBy, start, end, transactionKind);
            return _mapper.Map<TransactionPagedList<TransactionWithSplits>>(transactions);
        }

        public async Task<bool> importTransactionsFromCSV(IFormFile csvFile)
        {
            try
            {
                using (var reader = new StreamReader(csvFile.OpenReadStream()))
                using (var csv = new CsvReader(reader, CultureInfo.InvariantCulture))
                {
                    csv.Context.RegisterClassMap<TransactionMap>();

                    while (csv.Read())
                    {
                        try
                        {
                            var record = csv.GetRecord<CreateTransactionCommand>();

                            // check if transaction is valid
                            bool valid = true;

                            // valid DateTime formats
                            string[] formats = { "M/d/yyyy", "MM/dd/yyyy", "M/dd/yyyy", "MM/d/yyyy" };

                            if (record.Mcc == "")
                            {
                                record.Mcc = null;
                            }

                            if (record.Description == "")
                            {
                                record.Description = null;
                            }

                            if (record.BeneficiaryName == "")
                            {
                                record.BeneficiaryName = null;
                            }

                            // 
                            DateTime dateTime;
                            if (!DateTime.TryParseExact(record.Date, formats, CultureInfo.InvariantCulture, DateTimeStyles.None, out dateTime))
                            {
                                _logger.LogInformation("SKIPPED: Invalid date format. Transaction with ID = " + record.Id + " and beneficiary-name = " + record.BeneficiaryName);
                                valid = false;
                            }
                            else if (record.Direction != Direction.c && record.Direction != Direction.d)
                            {
                                _logger.LogInformation("SKIPPED: Invalid direction. Transaction with ID = " + record.Id + " and beneficiary-name = " + record.BeneficiaryName);
                                valid = false;
                            }
                            else if (string.IsNullOrEmpty(record.Currency) || !CurrencyCodesResolver.Codes.Select(c => c.Code).ToList().Contains(record.Currency.ToUpper()))
                            {
                                _logger.LogInformation("SKIPPED: Invalid currency code. Transaction with ID = " + record.Id + " and beneficiary-name = " + record.BeneficiaryName);
                                valid = false;
                            }
                            else if (record.Mcc != null && record.Mcc.Length != 4)
                            {
                                _logger.LogInformation("SKIPPED: Invalid mcc code. Transaction with ID = " + record.Id + " and beneficiary-name = " + record.BeneficiaryName);
                                valid = false;
                            }



                            if (valid)
                            {
                                var newRecordEntity = _mapper.Map<TransactionEntity>(record);
                                try
                                {
                                    await _repository.CreateTransaction(newRecordEntity);
                                }
                                catch (InvalidOperationException e)
                                {
                                    _logger.LogInformation("ERROR Transaction already exists: Transaction with ID = " + record.Id + " and beneficiary-name = " + record.BeneficiaryName);
                                }
                                catch (DbUpdateException e)
                                {
                                    _logger.LogInformation("DB Error: Transaction with ID = " + record.Id + " and beneficiary-name = " + record.BeneficiaryName);
                                }
                            }

                        }
                        catch (TypeConverterException e)
                        {
                            // TODO: Add more info about the skipped row
                            _logger.LogInformation("SKIPPED: Bad conversion." + e.Message);
                        }
                    }

                    return true;
                }

            }
            catch (HeaderValidationException ex)
            {
                _logger.LogError("ERROR: Wrong format of CSV header file!");
                return false;
            }
        }


        public async Task<int> CategorizeTransactionAsync(int transactionId, TransactionCategorizeCommand command)
        {
            // check if transactionId exists in the database
            var transaction = await _repository.GetTransactionById(transactionId);
            if (transaction == null)
            {
                return 2;
            }

            // check if catcode is valid catcode
            var category = await _categoryRepository.GetCategoryByCode(command.Catcode);
            if (category == null)
            {
                return 1;
            }

            // if all is okay return 0
            var value = await _repository.SetTransactionCategory(transaction, command.Catcode);

            return 0;
        }

        public async Task<int> SplitTransactionAsync(int transactionId, SplitTransactionCommand command)
        {

            // check if catcodes are valid
            int retval = 0;

            foreach ( var split in command.Splits)
            {
                var category = await _categoryRepository.GetCategoryByCode(split.Catcode);
                if (category == null)
                {
                    retval = 1;
                }
            }

            if (retval == 1)
            {
                return retval;
            }
            

            // check if transactionId exists in the database
            var transaction = await _repository.GetTransactionById(transactionId);
            if (transaction == null)
            {
                return 2;
            }

            // check if sum is valid
            double sum = 0;

            command.Splits.ForEach(split =>
            {
                sum += split.Amount;
            });

            if (sum != transaction.Amount)
            {
                return 3;
            }

            // if all is okay return 0
            var value = await _transactionSplitRepository.SplitTransaction(transaction, command);


            return 0;
        }
    }
}
