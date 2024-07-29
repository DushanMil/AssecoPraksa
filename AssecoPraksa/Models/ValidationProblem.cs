using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace AssecoPraksa.Models
{
    [DisplayName("validation-problem")]
    public class ValidationProblem
    {

        public class ProblemDetails
        {
            [Required]
            public string Tag { get; set; } = null!;
            [Required]
            public string Error { get; set; } = null!;
            [Required]
            public string Message { get; set; } = null!;

            public ProblemDetails(string tag, string error, string message)
            {
                Tag = tag;
                Error = error;
                Message = message;
            }
        }

        public List<ProblemDetails> Errors { get; set; } = null!;

        public ValidationProblem()
        {
            Errors = new List<ProblemDetails>();
        }
    }
}
