using AssecoPraksa.Database.Entities;
using AssecoPraksa.Models;

namespace AssecoPraksa.Database.Repositories
{
    public interface ITransactionSplitRepository
    {
        public Task<bool> DeleteTransactionSplit(TransactionEntity entity);
        public Task<TransactionSplitEntity> CreateTransactionSplit(TransactionSplitEntity newTransactionSplitEntity);
        public Task<TransactionEntity> SplitTransaction(TransactionEntity toSplitTransaction, SplitTransactionCommand command);
    }
}
