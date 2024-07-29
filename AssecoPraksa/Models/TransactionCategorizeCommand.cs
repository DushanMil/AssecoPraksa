using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace AssecoPraksa.Models
{
    [DisplayName("transaction-categorize-command")]
    public class TransactionCategorizeCommand
    {
        [Required]
        public string Catcode { get; set; }
    }
}
