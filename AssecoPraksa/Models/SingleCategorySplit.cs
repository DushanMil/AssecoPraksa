using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace AssecoPraksa.Models
{
    [DisplayName("single-category-split")]
    public class SingleCategorySplit
    {
        [Required]
        [DisplayName("catcode")]
        public string Catcode { get; set; } = null!;
        [Required]
        [DisplayName("amount")]
        public double Amount { get; set; }
    }
}
