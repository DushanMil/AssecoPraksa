using System.Numerics;
using AssecoPraksa.Models;
using AssecoPraksa.Services;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;

namespace AssecoPraksa.Controllers
{
    // using cors, this is an ApiController - it accepts http requests
    // routing requests starting with transaction
    [EnableCors("MyCORSPolicy")]
    [ApiController]
    [Route("transaction")]
    public class TransactionController : Controller
    {
        ITransactionService _transactionService;
        private readonly ILogger<TransactionsController> _logger;

        public TransactionController(ILogger<TransactionsController> logger, ITransactionService productService)
        {
            _logger = logger;
            _transactionService = productService;
        }

        // transaction/{transactionId}/split
        [HttpPost("{transactionId}/split")]
        public async Task<IActionResult> SplitTransactionAsync([FromRoute] string transactionId)
        {
            // ovo treba da ima neki asinhroni poziv
            // u telu zahteva postoji splits - na koje delove treba podeliti transakciju
            // split-transaction-command, from body, own model, TODO
            // TODO
            return Ok();
        }

        // transaction/{transactionId}/categorize
        [HttpPost("{transactionId}/categorize")]
        public async Task<IActionResult> CategorizeTransactionAsync([FromRoute] string transactionId, [FromBody] TransactionCategorizeCommand command)
        {
            int number;
            if (transactionId == null || string.IsNullOrEmpty(transactionId) || !int.TryParse(transactionId, out number))
            {
                // validation problem
                var problem = new ValidationProblem();
                problem.Errors.Add(new ValidationProblem.ProblemDetails("transactionId", "required", "Transaction id invalid id"));
                return BadRequest(problem);
            }

            if (command == null || string.IsNullOrEmpty(command.Catcode))
            {
                // validation problem
                var problem = new ValidationProblem();
                problem.Errors.Add(new ValidationProblem.ProblemDetails("transaction-categorize-command", "required", "Missing category code"));
                return BadRequest(problem);
            }

            var value = await _transactionService.CategorizeTransactionAsync(number, command);
            if (value == 0)
            {
                return Ok("OK - Transaction categorized");
            }
            else if (value == 1)
            {
                // business problem - category code is not a valid code
                return StatusCode(440, new BusinessProblem("m", "Category code doesn't exist", "Code " + command.Catcode + " doesn't exist"));

            }
            else if (value == 2)
            {
                // business problem - transactionId is not a valid id
                return StatusCode(440, new BusinessProblem("l", "Transaction id doesn't exist", "Transaction with id = " + transactionId + " doesn't exist"));
            }
            else
            {
                // idk
                return BadRequest("unknown problem");
            }

        }

        // transaction/auto-categorize
        [HttpPost("auto-categorize")]
        public async Task<IActionResult> AutoCategorizeAsync()
        {
            // nema parametara, samo radi autoCategorize
            // TODO
            return Ok();
        }



        // dont know what this is, look into it later
        protected IActionResult Index()
        {
            return View();
        }
    }
}
