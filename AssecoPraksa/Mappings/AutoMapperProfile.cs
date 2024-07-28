using System.Globalization;
using AssecoPraksa.Commands;
using AssecoPraksa.Database.Entities;
using AssecoPraksa.Models;
using AutoMapper;

namespace AssecoPraksa.Mappings
{
    public class AutoMapperProfile: Profile
    {
        public AutoMapperProfile() 
        {
            CreateMap<String, DateTime>().ConvertUsing<StringToDateTimeConverter>();
            CreateMap<DateTime, string>().ConvertUsing(dt => dt.ToString("M/d/yyyy", CultureInfo.InvariantCulture));
            // vrsi se mapiranje od CreateTransactionCommand u TransactionEntity
            // ne treba nam mapiranje membera jer se svi parametri isto zovu
            CreateMap<CreateTransactionCommand, TransactionEntity>();

            CreateMap<TransactionEntity, TransactionWithSplits>();
            CreateMap<TransactionWithSplits, TransactionEntity>();

            CreateMap<TransactionPagedList<TransactionEntity>, TransactionPagedList<TransactionWithSplits>>();

            CreateMap<CreateCategoryCommand, CategoryEntity>();

        }
    }
}
