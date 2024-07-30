using AssecoPraksa.Database.Entities;
using AssecoPraksa.Models;

namespace AssecoPraksa.Services
{
    public interface ICategoryService
    {
        public Task<Category?> GetCategoryByCode(string code);
        public Task<CategoryList<Category>?> getCategoryList(string? parentCode);
        public Task<bool> importCategoriesFromCSV(IFormFile csvFile);


    }
}
