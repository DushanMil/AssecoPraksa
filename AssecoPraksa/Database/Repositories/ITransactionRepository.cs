using AssecoPraksa.Database.Entities;
using AssecoPraksa.Models;

namespace AssecoPraksa.Database.Repositories
{
    public interface ITransactionRepository
    {
        public Task<TransactionPagedList<TransactionEntity>> GetTransactionsAsync(int page = 1, int pageSize = 10, SortOrder sortOrder = SortOrder.Asc, string? sortBy = null, DateTime? startDate = null, DateTime? endDate = null, string? transactionKind = null);

        public Task<List<TransactionEntity>> getAllTransactionsWithouthCatcodeAsync();

        public Task<TransactionEntity> CreateTransaction(TransactionEntity newTransactionEntity);

        public Task<TransactionEntity?> GetTransactionById(int transactionId);

        public Task<TransactionEntity> SetTransactionCategory(TransactionEntity transaction, string catcode);

        public Task<SpendingsByCategory> GetSpendingsByCategory(string? catcode = null, DateTime? startDate = null, DateTime? endDate = null, Direction? direction = null);

        public Task<bool> RunQueryForAutoCategorization(string query, string catcode);
    }
}
