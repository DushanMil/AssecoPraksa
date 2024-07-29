using AssecoPraksa.Database.Entities;
using AssecoPraksa.Models;

namespace AssecoPraksa.Database.Repositories
{
    public interface ICategoryRepository
    {
        public Task<CategoryEntity?> GetCategoryByCode(string code);

        public Task<CategoryEntity> UpdateCategory(CategoryEntity newCategoryEntity);

        public Task<CategoryEntity> CreateCategory(CategoryEntity newCategoryEntity);

        public Task<CategoryList<CategoryEntity>> GetCategories(string? parentCode);
    }
}
