using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace AssecoPraksa.Models
{
    [DisplayName("spendings-by-category")]
    public class SpendingsByCategory
    {
        [DisplayName("spending-in-category")]
        public class SpendingInCategory
        {
            
            public string? Catcode { get; set; }
           
            public double? Amount { get; set; }
            public int? Number { get; set; }

            public SpendingInCategory(string? catcode, double? amount, int? number)
            {
                Catcode = catcode;
                Amount = amount;
                Number = number;
            }
        }

        public List<SpendingInCategory> Groups { get; set; }

        public SpendingsByCategory()
        {
            Groups = new List<SpendingInCategory>();
        }
    }
}
