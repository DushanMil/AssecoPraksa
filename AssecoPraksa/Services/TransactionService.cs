
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

namespace AssecoPraksa.Services
{

    public class TransactionService : ITransactionService
    {
        ITransactionRepository _repository;
        IMapper _mapper;
        private readonly ILogger<TransactionService> _logger;

        public TransactionService(ILogger<TransactionService> logger, ITransactionRepository repostitory, IMapper mapper)
        {
            _repository = repostitory;
            _mapper = mapper;
            _logger = logger;
        }

        // helper function for parsing a CSV file
        // it returns entities that will be added to the database
        // only valid entites will be returned
        private List<TransactionEntity> ParseCsv(string csvData)
        {
            var records = new List<TransactionEntity>();
            using (var reader = new StringReader(csvData))
            {
                // first line should be skipped over, it is the header of csv file
                reader.ReadLine();

                string line;
                while ((line = reader.ReadLine()) != null)
                {
                    var values = line.Split(',');

                    // check if the transaction values are valid

                    Direction tDirection;
                    if (values[3] == "d")
                    {
                        tDirection = Direction.d;
                    }
                    else if (values[3] == "c")
                    {
                        tDirection = Direction.c;
                    }
                    else
                    {
                        // bad direction
                        tDirection = Direction.c;
                    }
                    
                    var entity = new TransactionEntity
                    {
                        Id = Int32.Parse(values[0]),
                        BeneficiaryName = values[1],
                        Date = values[2],
                        Direction = tDirection,
                        Amount = Double.Parse(values[4]),
                        Description = values[5],
                        Currency = values[6],
                        Mcc = values[7],
                        Catcode = values[8],
                    };

                    _logger.LogInformation(entity.Id.ToString() + " " + entity.BeneficiaryName);
                    
                    records.Add(entity);
                    
                }
            }
            return records;
        }


        public async Task<bool> importTransactionsFromCSV(IFormFile csvFile)
        {

            using (var reader = new StreamReader(csvFile.OpenReadStream()))
            using (var csv = new CsvReader(reader, CultureInfo.InvariantCulture))
            {
                csv.Context.RegisterClassMap<TransactionMap>();
                var records = csv.GetRecords<CreateTransactionCommand>();

                // save to database
                foreach (var record in records)
                {
                    var newRecordEntity = _mapper.Map<TransactionEntity>(record);
                    await _repository.CreateTransaction(newRecordEntity);

                }
            }

            return true;
        }
    }
}
