namespace AssecoPraksa.Services
{
    public interface ITransactionService
    {
        Task<bool> importTransactionsFromCSV(IFormFile csvFile);
    }
}
