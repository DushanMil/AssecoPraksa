using System.ComponentModel;

namespace AssecoPraksa.Models
{
    [DisplayName("business-problem")]
    public class BusinessProblem
    {
        public string? Problem {  get; set; }
        public string? Message { get; set; }
        public string? Details { get; set; }

        public BusinessProblem(string? problem, string? message, string? details)
        {
            Problem = problem;
            Message = message;
            Details = details;
        }
    }
}
