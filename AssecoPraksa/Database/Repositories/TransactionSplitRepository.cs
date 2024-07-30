using AssecoPraksa.Database.Entities;
using AssecoPraksa.Models;
using Microsoft.EntityFrameworkCore;

namespace AssecoPraksa.Database.Repositories
{
    public class TransactionSplitRepository : ITransactionSplitRepository
    {
        TransactionDbContext _dbContext;

        public TransactionSplitRepository(TransactionDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<bool> DeleteTransactionSplit(TransactionEntity entity)
        {
            _dbContext.TransactionSplits.RemoveRange(_dbContext.TransactionSplits.Where(x => x.TransactionId == entity.Id));
            await _dbContext.SaveChangesAsync();

            return true;
        }

        public async Task<TransactionSplitEntity> CreateTransactionSplit(TransactionSplitEntity newTransactionSplitEntity)
        {
            _dbContext.TransactionSplits.Add(newTransactionSplitEntity);
            await _dbContext.SaveChangesAsync();

            return newTransactionSplitEntity;
        }

        public async Task<TransactionEntity> SplitTransaction(TransactionEntity toSplitTransaction, SplitTransactionCommand command)
        {
            await DeleteTransactionSplit(toSplitTransaction);

            // add a transaction split forEach split in command
            foreach (var item in command.Splits)
            {
                var newTransactionSplit = new TransactionSplitEntity
                {
                    TransactionId = toSplitTransaction.Id,
                    Catcode = item.Catcode,
                    Amount = item.Amount
                };

                await CreateTransactionSplit(newTransactionSplit);
            }


            return toSplitTransaction;
        }

        public async Task<List<TransactionSplitEntity>> GetTransactionSplits(int transactionId)
        {
            var query = _dbContext.TransactionSplits.AsQueryable();

            query = query.Where(transactionSplit => transactionSplit.TransactionId == transactionId);

            var splits = query.ToList();

            return splits;
        }

    }
}
