using AssecoPraksa.Models;

namespace AssecoPraksa.Services
{
    public interface ICategoryService
    {
        public Task<CategoryList<Category>> getCategoryList(string? parentCode);
        public Task<bool> importCategoriesFromCSV(IFormFile csvFile);


    }
}
