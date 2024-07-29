using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace AssecoPraksa.Models
{
    [DisplayName("split-transaction-command")]
    public class SplitTransactionCommand
    {
        [DisplayName("single-category-split")]
        public class SingleCategorySplit
        {
            [Required]
            public string Catcode { get; set; } = null!;
            [Required]
            public double Amount { get; set; }

            public SingleCategorySplit(string catcode, double amount)
            {
                Catcode = catcode;
                Amount = amount;
            }
        }

        public List<SingleCategorySplit> Splits { get; set; }

        public SplitTransactionCommand()
        {
            Splits = new List<SingleCategorySplit>();
        }
    }
}
