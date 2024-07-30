
using AssecoPraksa.Commands;
using AssecoPraksa.Database.Entities;
using AssecoPraksa.Database.Repositories;
using AssecoPraksa.Mappings;
using AssecoPraksa.Models;
using AutoMapper;
using CsvHelper;
using CsvHelper.TypeConversion;
using Microsoft.EntityFrameworkCore;
using System.Globalization;

namespace AssecoPraksa.Services
{
    public class CategoryService : ICategoryService
    {
        private readonly ILogger<TransactionService> _logger;
        ICategoryRepository _repository;
        IMapper _mapper;

        public CategoryService(ILogger<TransactionService> logger, ICategoryRepository repostitory, IMapper mapper)
        {
            _repository = repostitory;
            _mapper = mapper;
            _logger = logger;
        }

        public async Task<Category?> GetCategoryByCode(string code)
        {
            var category = await _repository.GetCategoryByCode(code);
            return _mapper.Map<Category>(category);
        }


        public async Task<CategoryList<Category>?> getCategoryList(string? parentCode)
        {
            // if parent code is set check if it is a valid parent code
            if (!string.IsNullOrEmpty(parentCode))
            {
                var category = await _repository.GetCategoryByCode(parentCode);
                if (category == null)
                {
                    return null;
                }
            }

            var categories =  await _repository.GetCategories(parentCode);
            return _mapper.Map<CategoryList<Category>>(categories);
        }

        public async Task<bool> importCategoriesFromCSV(IFormFile csvFile)
        {
            try
            {
                using (var reader = new StreamReader(csvFile.OpenReadStream()))
                using (var csv = new CsvReader(reader, CultureInfo.InvariantCulture))
                {

                    csv.Context.RegisterClassMap<CategoryMap>();

                    while (csv.Read())
                    {
                        try
                        {
                            var record = csv.GetRecord<CreateCategoryCommand>();

                            bool valid = true;

                            if (record.Code == "")
                            {
                                _logger.LogInformation("SKIPPED: Category Code can't be null. Category with Code = " + record.Code + " and parent-code = " + record.ParentCode);
                                valid = false;
                            }

                            if (record.ParentCode == "")
                            {
                                record.ParentCode = null;
                            }

                            if (record.Name == "")
                            {
                                record.Name = null;
                            }

                            if (valid)
                            {
                                try
                                {
                                    var newRecordEntity = _mapper.Map<CategoryEntity>(record);
                                    // check if category already exists
                                    var category = await _repository.GetCategoryByCode(record.Code);
                                    if (category == null)
                                    {
                                        // category doesnt exist, add it to the table
                                        
                                        await _repository.CreateCategory(newRecordEntity);
                                    }
                                    else
                                    {
                                        // update category
                                        await _repository.UpdateCategory(newRecordEntity);
                                    }


                                }
                                catch (DbUpdateException e)
                                {
                                    _logger.LogInformation("DB Error: Category with Code = " + record.Code + " and parent-code = " + record.ParentCode);
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
    }
}
