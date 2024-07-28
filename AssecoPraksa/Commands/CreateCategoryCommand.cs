using AssecoPraksa.Models;
using System.ComponentModel.DataAnnotations;

namespace AssecoPraksa.Commands
{
    public class CreateCategoryCommand
    {
        public string? Code{ get; set; }
        public string? ParentCode { get; set; }
        
        public string? Name { get; set; }
    }
}
