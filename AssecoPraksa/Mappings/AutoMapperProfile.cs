using AssecoPraksa.Commands;
using AssecoPraksa.Database.Entities;
using AutoMapper;

namespace AssecoPraksa.Mappings
{
    public class AutoMapperProfile: Profile
    {
        public AutoMapperProfile() 
        {
            // vrsi se mapiranje od CreateTransactionCommand u TransactionEntity
            // ne treba nam mapiranje membera jer se svi parametri isto zovu
            CreateMap<CreateTransactionCommand, TransactionEntity>();
                
        }
    }
}
