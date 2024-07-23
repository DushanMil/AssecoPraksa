using AssecoPraksa.Database.Entities;

namespace AssecoPraksa.Database.Repositories
{
    public interface ITransactionRepository
    {
        Task<TransactionEntity> CreateTransaction(TransactionEntity newTransactionEntity);
    }
}
