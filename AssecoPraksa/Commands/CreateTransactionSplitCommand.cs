using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace AssecoPraksa.Commands
{
    public class CreateTransactionSplitCommand
    {
        [Required]
        public int TransactionId { get; set; }
        [Required]
        public int SplitId { get; set; }

        public string? Catcode { get; set; }
        [Required]
        public double Amount { get; set; }
    }
}
