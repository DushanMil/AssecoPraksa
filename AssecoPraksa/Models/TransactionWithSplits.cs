using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace AssecoPraksa.Models
{
    public class TransactionWithSplits
    {
        [Required]
        [DisplayName("id")]
        public string Id { get; set; } = null!;
        [DisplayName("beneficiary-name")]
        public string? BeneficiaryName { get; set; }
        [Required]
        [DisplayName("date")]
        public string Date { get; set; } = null!;
        [Required]
        public Direction Direction { get; set; }
        [Required]
        public double Amount { get; set; }
        public string? Description { get; set; }
        [Required]
        [StringLength(3, MinimumLength = 3, ErrorMessage = "Currency has to be of length 3")]
        public string Currency { get; set; } = null!;
        [StringLength(4, MinimumLength = 4, ErrorMessage = "mcc has to be of length 4")]
        public string? Mcc { get; set; }
        [Required]
        public TransactionKind TransactionKind { get; set; }
        public string? Catcode { get; set; }
        public List<SplitTransactionCommand.SingleCategorySplit> Splits { get; set; }

        public TransactionWithSplits()
        {
            Splits = new List<SplitTransactionCommand.SingleCategorySplit>();
        }

    }
}
