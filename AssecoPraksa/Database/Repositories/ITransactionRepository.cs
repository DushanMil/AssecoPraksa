using AssecoPraksa.Database.Entities;
using AssecoPraksa.Models;

namespace AssecoPraksa.Database.Repositories
{
    public interface ITransactionRepository
    {
        public Task<TransactionPagedList<TransactionEntity>> GetTransactionsAsync(int page = 1, int pageSize = 10, SortOrder sortOrder = SortOrder.Asc, string? sortBy = null, DateTime? startDate = null, DateTime? endDate = null, string? transactionKind = null);
        public Task<TransactionEntity> CreateTransaction(TransactionEntity newTransactionEntity);
    }
}
