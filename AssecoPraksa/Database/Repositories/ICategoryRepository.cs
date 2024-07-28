using AssecoPraksa.Database.Entities;

namespace AssecoPraksa.Database.Repositories
{
    public interface ICategoryRepository
    {
        public Task<CategoryEntity> GetCategoryByCode(string code);

        public Task<CategoryEntity> UpdateCategory(CategoryEntity newCategoryEntity);

        public Task<CategoryEntity> CreateCategory(CategoryEntity newCategoryEntity);
    }
}
