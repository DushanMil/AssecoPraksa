using AssecoPraksa.Database.Entities;
using Microsoft.EntityFrameworkCore;

namespace AssecoPraksa.Database.Repositories
{
    public class CategoryRepository : ICategoryRepository
    {
        TransactionDbContext _dbContext;

        public CategoryRepository(TransactionDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<CategoryEntity> GetCategoryByCode(string code)
        {
            return await _dbContext.Categories.FirstOrDefaultAsync(x => x.Code.Equals(code));

        }

        public async Task<CategoryEntity> CreateCategory(CategoryEntity newCategoryEntity)
        {
            _dbContext.Categories.Add(newCategoryEntity);
            await _dbContext.SaveChangesAsync();

            return newCategoryEntity;

        }

        public async Task<CategoryEntity> UpdateCategory(CategoryEntity newCategoryEntity)
        {
            var result = await _dbContext.Categories.FirstOrDefaultAsync(x => x.Code.Equals(newCategoryEntity.Code));

            if (result != null)
            {
                result.ParentCode = newCategoryEntity.ParentCode;
                result.Name = newCategoryEntity.Name;
                _dbContext.SaveChanges();
            }

            return newCategoryEntity;
        }
    }
}
