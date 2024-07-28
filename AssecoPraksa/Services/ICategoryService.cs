namespace AssecoPraksa.Services
{
    public interface ICategoryService
    {
        public Task<bool> importCategoriesFromCSV(IFormFile csvFile);
    }
}
