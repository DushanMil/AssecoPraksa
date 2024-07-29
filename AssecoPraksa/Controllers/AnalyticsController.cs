using System.Globalization;
using AssecoPraksa.Models;
using AssecoPraksa.Services;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory.Database;

namespace AssecoPraksa.Controllers
{
    [EnableCors("MyCORSPolicy")]
    [ApiController]
    [Route("spending-analytics")]
    public class AnalyticsController : Controller
    {
        ITransactionService _transactionService;
        private readonly ILogger<TransactionsController> _logger;

        public AnalyticsController(ILogger<TransactionsController> logger, ITransactionService productService)
        {
            _logger = logger;
            _transactionService = productService;
        }


        [HttpGet]
        public async Task<IActionResult> GetSpendingAnalyticsAsync(
            [FromQuery] string? catcode = null,
            [FromQuery(Name = "start-date")] string? startDate = null,
            [FromQuery(Name = "end-date")] string? endDate = null,
            [FromQuery] Direction? direction = null)
        {
            // check if catcode is valid is done in the TransactionService

            // API controller checks if the direction is valid

            // check if dates have a valid format
            string[] formats = { "M/d/yyyy", "MM/dd/yyyy", "M/dd/yyyy", "MM/d/yyyy" };

            DateTime startDateTime;
            if (!string.IsNullOrEmpty(startDate) && !DateTime.TryParseExact(startDate, formats, CultureInfo.InvariantCulture, DateTimeStyles.None, out startDateTime))
            {
                var problem = new ValidationProblem();
                problem.Errors.Add(new ValidationProblem.ProblemDetails("start-date", "invalid-format", "Bad start-date format"));
                return BadRequest(problem);
            }

            DateTime endDateTime;
            if (!string.IsNullOrEmpty(endDate) && !DateTime.TryParseExact(endDate, formats, CultureInfo.InvariantCulture, DateTimeStyles.None, out endDateTime))
            {
                var problem = new ValidationProblem();
                problem.Errors.Add(new ValidationProblem.ProblemDetails("end-date", "invalid-format", "Bad end-date format"));
                return BadRequest(problem);
            }

            SpendingsByCategory? spendings;

            if (!string.IsNullOrEmpty(startDate) && !string.IsNullOrEmpty(endDate))
            {
                // both strings are not null and valid
                spendings = await _transactionService.GetSpendingsByCategory(catcode,
                    DateTime.ParseExact(startDate, formats, CultureInfo.InvariantCulture, DateTimeStyles.None).ToUniversalTime(), 
                    DateTime.ParseExact(endDate, formats, CultureInfo.InvariantCulture, DateTimeStyles.None).ToUniversalTime(), 
                    direction);

            }
            else if (!string.IsNullOrEmpty(startDate) && string.IsNullOrEmpty(endDate))
            {
                // start date is not null and valid
                // end date is null
                spendings = await _transactionService.GetSpendingsByCategory(catcode,
                    DateTime.ParseExact(startDate, formats, CultureInfo.InvariantCulture, DateTimeStyles.None).ToUniversalTime(), 
                    DateTime.Now.ToUniversalTime(), direction);
            }
            else if (string.IsNullOrEmpty(startDate) && !string.IsNullOrEmpty(endDate))
            {
                // start date is null
                // end date is not null
                spendings = await _transactionService.GetSpendingsByCategory(catcode,
                    DateTime.MinValue.ToUniversalTime(),
                    DateTime.ParseExact(endDate, formats, CultureInfo.InvariantCulture, DateTimeStyles.None).ToUniversalTime(),
                    direction);
            }
            else
            {
                // both fields are null or empty
                spendings = await _transactionService.GetSpendingsByCategory(catcode,
                    null, null, direction);

            }

            if (spendings == null)
            {
                // business problem - category code is not a valid code
                return StatusCode(440, new BusinessProblem("m", "Category code doesn't exist", "Code " + catcode + " doesn't exist"));
            }

            return Ok(spendings);
        }

        protected IActionResult Index()
        {
            return View();
        }
    }
}
