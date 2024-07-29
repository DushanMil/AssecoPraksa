using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace AssecoPraksa.Database.Entities
{
    public class TransactionSplitEntity
    {
        [Column("transaction-id")]
        public int TransactionId { get; set; }
        [Column("split-id")]
        public int SplitId { get; set; }

        public string? Catcode { get; set; }
        [Required]
        public double Amount { get; set; }

        [Required]
        public TransactionEntity Transaction { get; set; } = null!;

        public CategoryEntity? Category {  get; set; } 
    }
}
