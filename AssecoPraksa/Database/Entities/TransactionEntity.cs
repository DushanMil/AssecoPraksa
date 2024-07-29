using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using AssecoPraksa.Models;

namespace AssecoPraksa.Database.Entities
{
    public class TransactionEntity
    {
        public int Id { get; set; }
        [Column("beneficiary-name")]
        public string? BeneficiaryName  { get; set; }
        [Required]
        public DateTime Date { get; set; }
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

        public CategoryEntity? Category { get; set; }
    }
}
