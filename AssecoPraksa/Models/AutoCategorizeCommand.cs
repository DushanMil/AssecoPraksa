using System.ComponentModel.DataAnnotations;

namespace AssecoPraksa.Models
{
    public class AutoCategorizeCommand
    {
        [Required]
        public string Title { get; set; } = null!;
        [Required]
        public string Catcode { get; set; } = null!;
        [Required]
        public string Predicate { get; set; } = null!;
    }
}
