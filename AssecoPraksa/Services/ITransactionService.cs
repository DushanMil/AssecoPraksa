using AssecoPraksa.Models;

namespace AssecoPraksa.Services
{
    public interface ITransactionService
    {
        public Task<TransactionPagedList<TransactionWithSplits>> getTransactionsAsync(int page, int pageSize, SortOrder sortOrder, string? sortBy, DateTime? start = null, DateTime? end = null, string? transactionKind = null);


        public Task<bool> importTransactionsFromCSV(IFormFile csvFile);

        public Task<int> CategorizeTransactionAsync(int transactionId, TransactionCategorizeCommand command);

        public Task<int> SplitTransactionAsync(int transactionId, SplitTransactionCommand command);
    }
}
