using AssecoPraksa.Database.Entities;

namespace AssecoPraksa.Database.Repositories
{
    public class TransactionRepository : ITransactionRepository
    {
        TransactionDbContext _dbContext;

        public TransactionRepository(TransactionDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        // dodavanje instrukcija
        public async Task<TransactionEntity> CreateTransaction(TransactionEntity newTransactionEntity)
        {
            _dbContext.Transactions.Add(newTransactionEntity);
            await _dbContext.SaveChangesAsync();

            return newTransactionEntity;
        }
    }
}
