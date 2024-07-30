using AssecoPraksa.Database.Entities;
using AssecoPraksa.Models;
using System.Data;
using Microsoft.EntityFrameworkCore;
using System.Runtime.Intrinsics.Arm;
using static AssecoPraksa.Models.SpendingsByCategory;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Globalization;

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

        private TransactionKind GetTransactionKind(string transactionKind)
        {
            TransactionKind helperTransactionKind;
            switch (transactionKind)
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

            return helperTransactionKind;
        }

        public async Task<TransactionPagedList<TransactionEntity>> GetTransactionsAsync(int page, int pageSize, SortOrder sortOrder, string? sortBy, DateTime? startDate, DateTime? endDate, string? transactionKind)
        {
            var query = _dbContext.Transactions.AsQueryable();
            var totalCount = query.Count();
            var totalPages = (int)Math.Ceiling(totalCount * 1.0 / pageSize);

            // filter query by transaction Kind
            if (!string.IsNullOrEmpty(transactionKind))
            {
                // all transaction kinds are valid
                string[] kinds = transactionKind.Split(',');
                List<TransactionKind> filterKinds = new List<TransactionKind>();
                foreach (string kind in kinds)
                {
                    TransactionKind helperTransactionKind = GetTransactionKind(kind);
                    filterKinds.Add(helperTransactionKind);
                    
                }
                Console.WriteLine(filterKinds);
                
                query = query.Where(transaction => filterKinds.Contains(transaction.TransactionKind));
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
            

            return new TransactionPagedList<TransactionEntity>
            {
                TotalCount = totalCount,
                PageSize = pageSize,
                Page = page,
                TotalPages = (transactions.Count() + pageSize - 1) / pageSize,
                SortOrder = sortOrder,
                SortBy = sortBy,
                Items = transactions
            };
        }

        private async Task<List<int>> GetSplittedTransactionIds()
        {
            var querySplitted = _dbContext.TransactionSplits.AsQueryable();

            var splittedTransactions = await querySplitted.ToListAsync();

            List<int> splittedTransactionsIds = new List<int>();

            foreach (var split in splittedTransactions)
            {
                if (!splittedTransactionsIds.Contains(split.TransactionId))
                {
                    splittedTransactionsIds.Add(split.TransactionId);
                }
            }

            return splittedTransactionsIds;
        }

        public async Task<SpendingsByCategory> GetSpendingsByCategory(string? catcode = null, DateTime? startDate = null, DateTime? endDate = null, Direction? direction = null)
        {
            // query for transactions left join categories
            var query = _dbContext.Transactions.Include(transaction => transaction.Category).AsQueryable();


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

            // if catcode is null then return grouping by top level category
            // if catcode is top level then return grouping by lower level category
            // if catcode is lower level category return grouping by low level category(doesn't exist)
            if (!string.IsNullOrEmpty(catcode))
            {
                // category is not null and valid. We need transactions which have a parent code equal to the parameter
                query = query.Where(transaction => (transaction.Category != null && transaction.Category.ParentCode == catcode) || (transaction.Category != null && transaction.Category.Code == catcode));
            }
            else
            {
                // category parameter is null or "". We need transactions which have parent code null
                query = query.Where(transaction => transaction.Category == null || string.IsNullOrEmpty( transaction.Category.ParentCode));
            }

            // we have to exclude transactions which have splits

            var splittedTransactionsIds = await GetSplittedTransactionIds();

            query = query.Where(transaction => !splittedTransactionsIds.Contains(transaction.Id));

            // now we can perform the query
            // we group by transaction categoryCode

            var spending = await query.GroupBy(transaction => transaction.Catcode).Select(group => new SpendingInCategory
            (
                group.Key,
                group.Sum(transaction => transaction.Amount),
                group.Count()
            )).ToListAsync();

            foreach (var split in spending)
            {
                Console.WriteLine(split.Catcode + " " + split.Amount + " " + split.Number);
            }


            // we have to perform a similar query for table of splitted transactions
            // we include the original transaction of the transaction split and the category of the transaction split
            var querySplitTransactions = _dbContext.TransactionSplits.Include(split => split.Category).Include(split => split.Transaction).AsQueryable();

            if (startDate != null && endDate != null)
            {
                querySplitTransactions = querySplitTransactions.Where(split => split.Transaction.Date >= startDate && split.Transaction.Date <= endDate);
            }

            // filter by direction
            if (direction != null)
            {
                querySplitTransactions = querySplitTransactions.Where(split => split.Transaction.Direction == direction);
            }

            // if catcode is null then return grouping by top level category
            // if catcode is top level then return grouping by lower level category
            // if catcode is lower level category return grouping by low level category(doesn't exist)
            if (!string.IsNullOrEmpty(catcode))
            {
                // category is not null and valid. We need transactions which have a parent code equal to the parameter
                // and transactions where code is catcode
                querySplitTransactions = querySplitTransactions.Where(split => (split.Category != null && split.Category.ParentCode == catcode) || (split.Category != null && split.Category.Code == catcode));
            }
            else
            {
                // category parameter is null or "". We need transactions which have parent code null
                querySplitTransactions = querySplitTransactions.Where(split => split.Category == null || string.IsNullOrEmpty(split.Category.ParentCode));
            }

            var spendingSplits = await querySplitTransactions.GroupBy(split => split.Catcode).Select(group => new SpendingInCategory
            (
                group.Key,
                group.Sum(transaction => transaction.Amount),
                group.Count()
            )).ToListAsync();

            

            // these two lists need to be combined
            // go over the splits list and check if a same category name exists in the category lists
            foreach(var categorySplit in spendingSplits)
            {
                bool found = false;

                foreach (var category in spending)
                {
                    if (category.Catcode == categorySplit.Catcode)
                    {
                        category.Amount += categorySplit.Amount;
                        category.Number += categorySplit.Number;
                        found = true;
                    }
                }

                if (!found)
                {
                    spending.Add(categorySplit);
                }
            }


            var retval = new SpendingsByCategory
            {
                Groups = spending
            };

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
