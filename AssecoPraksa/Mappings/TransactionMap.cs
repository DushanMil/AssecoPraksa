using AssecoPraksa.Commands;
using CsvHelper.Configuration;

namespace AssecoPraksa.Mappings
{
    public class TransactionMap: ClassMap<CreateTransactionCommand>
    {
        public TransactionMap()
        {
            Map(p => p.Id).Index(0).Name("id");
            Map(p => p.BeneficiaryName).Index(1).Name("beneficiary-name");
            Map(p => p.Date).Index(2).Name("date");
            Map(p => p.Direction).Index(3).Name("direction");
            Map(p => p.Amount).Index(4).Name("amount");
            Map(p => p.Description).Index(5).Name("description");
            Map(p => p.Currency).Index(5).Name("currency");
            Map(p => p.Mcc).Index(6).Name("mcc");
            Map(p => p.TransactionKind).Index(7).Name("kind");
            Map(p => p.Catcode).Index(8).Name("catcode").Optional();
        } 
    }
}
