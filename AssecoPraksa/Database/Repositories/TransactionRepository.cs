using AssecoPraksa.Database.Entities;
using AssecoPraksa.Models;
using System.Data;
using Microsoft.EntityFrameworkCore;
using System.Runtime.Intrinsics.Arm;
using static AssecoPraksa.Models.SpendingsByCategory;

namespace AssecoPraksa.Database.Repositories
{
    public class TransactionRepository : ITransactionRepository
    {
        TransactionDbContext _dbContext;

        public TransactionRepository(TransactionDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<TransactionEntity?> GetTransactionById(int transactionId)
        {
            return await _dbContext.Transactions.FirstOrDefaultAsync(x => x.Id.Equals(transactionId));
        }

        public async Task<TransactionPagedList<TransactionEntity>> GetTransactionsAsync(int page, int pageSize, SortOrder sortOrder, string? sortBy, DateTime? startDate, DateTime? endDate, string? transactionKind)
        {
            var query = _dbContext.Transactions.AsQueryable();
            var totalCount = query.Count();
            var totalPages = (int)Math.Ceiling(totalCount * 1.0 / pageSize);

            // filter query by transaction Kind
            if (!string.IsNullOrEmpty(transactionKind))
            {
                TransactionKind helperTransactionKind;
                switch(transactionKind)
                {
                    case "dep":
                        helperTransactionKind = TransactionKind.dep;
                        break;
                    case "wdw":
                        helperTransactionKind = TransactionKind.wdw;
                        break;
                    case "pmt":
                        helperTransactionKind = TransactionKind.pmt;
                        break;
                    case "fee":
                        helperTransactionKind = TransactionKind.fee;
                        break;
                    case "inc":
                        helperTransactionKind = TransactionKind.inc;
                        break;
                    case "rev":
                        helperTransactionKind = TransactionKind.rev;
                        break;
                    case "adj":
                        helperTransactionKind = TransactionKind.adj;
                        break;
                    case "lnd":
                        helperTransactionKind = TransactionKind.lnd;
                        break;
                    case "lnr":
                        helperTransactionKind = TransactionKind.lnr;
                        break;
                    case "fcx":
                        helperTransactionKind = TransactionKind.fcx;
                        break;
                    case "aop":
                        helperTransactionKind = TransactionKind.aop;
                        break;
                    case "acl":
                        helperTransactionKind = TransactionKind.acl;
                        break;
                    case "spl":
                        helperTransactionKind = TransactionKind.spl;
                        break;
                    case "sal":
                        helperTransactionKind = TransactionKind.sal;
                        break;
                    default:
                        helperTransactionKind = TransactionKind.dep;
                        break;
                }
                query = query.Where(transaction => transaction.TransactionKind == helperTransactionKind);
            }

            // filter query by date
            // startDate and endDate either are both null or both are valid dates
            if (startDate != null && endDate != null)
            {
                query = query.Where(transaction => transaction.Date >= startDate && transaction.Date <= endDate);
            } 

            if (!String.IsNullOrEmpty(sortBy))
            {
                switch (sortBy)
                {
                    case "id":
                        query = sortOrder == SortOrder.Asc ? query.OrderBy(x => x.Id) : query.OrderByDescending(x => x.Id);
                        break;
                    case "beneficiary-name":
                        query = sortOrder == SortOrder.Asc ? query.OrderBy(x => x.BeneficiaryName) : query.OrderByDescending(x => x.BeneficiaryName);
                        break;
                    case "date":
                        query = sortOrder == SortOrder.Asc ? query.OrderBy(x => x.Date) : query.OrderByDescending(x => x.Date);
                        break;
                    case "direction":
                        query = sortOrder == SortOrder.Asc ? query.OrderBy(x => x.Direction) : query.OrderByDescending(x => x.Direction);
                        break;
                    case "amount":
                        query = sortOrder == SortOrder.Asc ? query.OrderBy(x => x.Amount) : query.OrderByDescending(x => x.Amount);
                        break;
                    case "description":
                        query = sortOrder == SortOrder.Asc ? query.OrderBy(x => x.Description) : query.OrderByDescending(x => x.Description);
                        break;
                    case "currency":
                        query = sortOrder == SortOrder.Asc ? query.OrderBy(x => x.Currency) : query.OrderByDescending(x => x.Currency);
                        break;
                    case "mcc":
                        query = sortOrder == SortOrder.Asc ? query.OrderBy(x => x.Mcc) : query.OrderByDescending(x => x.Mcc);
                        break;
                    case "transaction-kind":
                        query = sortOrder == SortOrder.Asc ? query.OrderBy(x => x.TransactionKind) : query.OrderByDescending(x => x.TransactionKind);
                        break;
                    case "catcode":
                        query = sortOrder == SortOrder.Asc ? query.OrderBy(x => x.Catcode) : query.OrderByDescending(x => x.Catcode);
                        break;
                }
            }
            else
            {
                query = query.OrderBy(x => x.Id);
            }
            query = query.Skip((page - 1) * pageSize).Take(pageSize);

            var transactions = await query.ToListAsync();

            // TODO: treba procitati i splitove transakcija
            

            return new TransactionPagedList<TransactionEntity>
            {
                TotalCount = transactions.Count(),
                PageSize = pageSize,
                Page = page,
                TotalPages = (transactions.Count() + pageSize - 1) / pageSize,
                SortOrder = sortOrder,
                SortBy = sortBy,
                Items = transactions
            };
        }

        public async Task<SpendingsByCategory> GetSpendingsByCategory(string? catcode = null, DateTime? startDate = null, DateTime? endDate = null, Direction? direction = null)
        {
            var query = _dbContext.Transactions.AsQueryable();


            // filter query by date
            // startDate and endDate either are both null or both are valid dates
            if (startDate != null && endDate != null)
            {
                query = query.Where(transaction => transaction.Date >= startDate && transaction.Date <= endDate);
            }

            // filter by direction
            if (direction != null)
            {
                query = query.Where(transaction => transaction.Direction == direction);
            }

            // filter by catcode
            if (!string.IsNullOrEmpty(catcode))
            {
                query = query.Where(transaction => transaction.Catcode == catcode);
            }



            var spending = query.GroupBy(transaction => transaction.Catcode).Select(group => new SpendingInCategory
            (
                group.Key,
                group.Sum(transaction => transaction.Amount),
                group.Count()
            )).ToList();

            var retval = new SpendingsByCategory();
            retval.Groups = spending;

            return retval;
        }



        // dodavanje instrukcija
        public async Task<TransactionEntity> CreateTransaction(TransactionEntity newTransactionEntity)
        {
            _dbContext.Transactions.Add(newTransactionEntity);
            await _dbContext.SaveChangesAsync();

            return newTransactionEntity;
        }
        // postavljanje kategorije instrukcije
        public async Task<TransactionEntity> SetTransactionCategory(TransactionEntity transaction, string catcode)
        {
            transaction.Catcode = catcode;
            await _dbContext.SaveChangesAsync();

            return transaction;
        }
    }
}
