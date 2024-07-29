using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace AssecoPraksa.Database.Entities
{
    public class CategoryEntity
    {
        [Column("code")]
        public string? Code { get; set; }
        [Column("parent-code")]
        public string? ParentCode { get; set; }
        [Column("name")]
        public string? Name { get; set; }

        public virtual ICollection<TransactionEntity>? Transactions { get; set; }
    }
}
