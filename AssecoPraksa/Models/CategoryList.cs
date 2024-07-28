using System.ComponentModel;

namespace AssecoPraksa.Models
{
    [DisplayName("category-list")]
    public class CategoryList<T>
    {
        public List<T> Items { get; set; }
    }
}
