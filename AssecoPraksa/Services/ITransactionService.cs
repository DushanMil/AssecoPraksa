using AssecoPraksa.Models;

namespace AssecoPraksa.Services
{
    public interface ITransactionService
    {
        public Task<TransactionPagedList<TransactionWithSplits>> getTransactionsAsync(int page, int pageSize, SortOrder sortOrder, string? sortBy, DateTime? start = null, DateTime? end = null, string? transactionKind = null);


        public Task<bool> importTransactionsFromCSV(IFormFile csvFile);
    }
}
