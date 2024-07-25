using AssecoPraksa.Models;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace AssecoPraksa.Commands
{
    public class CreateTransactionCommand
    {
        public int Id { get; set; }
        public string? BeneficiaryName { get; set; }
        [Required]
        public string Date { get; set; } = null!;
        [Required]
        public Direction Direction { get; set; }
        [Required]
        public double Amount { get; set; }
        public string? Description { get; set; }
        [Required]
        public string Currency { get; set; } = null!;
        public string? Mcc { get; set; }
        [Required]
        public TransactionKind TransactionKind { get; set; }
        public string? Catcode { get; set; }
    }
}
