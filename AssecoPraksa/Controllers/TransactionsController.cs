using System.Globalization;
using System;
using AssecoPraksa.Models;
using AssecoPraksa.Services;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;
using System.Runtime.Intrinsics.Arm;

namespace AssecoPraksa.Controllers
{
    [EnableCors("MyCORSPolicy")]
    [ApiController]
    [Route("transactions")]
    public class TransactionsController : Controller
    {
        // za sad neka ga tu logger, ne znam bas sta ce mi niti kako radi
        ITransactionService _transactionService;
        private readonly ILogger<TransactionsController> _logger;

        public TransactionsController(ILogger<TransactionsController> logger, ITransactionService productService)
        {
            _logger = logger;
            _transactionService = productService;
        }

        [HttpGet]
        public async Task<IActionResult> GetTransactionsAsync(
            [FromQuery(Name = "transaction-kind")] string? transactionKind = null, 
            [FromQuery(Name = "start-date")] string? startDate = null, 
            [FromQuery(Name = "end-date")] string? endDate = null, 
            [FromQuery] int page = 1, 
            [FromQuery(Name = "page-size")] int pageSize = 10, 
            [FromQuery(Name = "sort-by")] string? sortBy = null, 
            [FromQuery(Name = "sort-order")] SortOrder sortOrder = SortOrder.Asc)
        {
            // all parameters either have a default value or can be null
            // response should be transaction-paged-list

            var problem = new ValidationProblem();

            string[] formats = { "M/d/yyyy", "MM/dd/yyyy", "M/dd/yyyy", "MM/d/yyyy" };

            DateTime startDateTime;
            if (!string.IsNullOrEmpty(startDate) && !DateTime.TryParseExact(startDate, formats, CultureInfo.InvariantCulture, DateTimeStyles.None, out startDateTime))
            {
                
                problem.Errors.Add(new ValidationProblem.ProblemDetails("start-date", "invalid-format", "Bad start-date format"));
            }

            DateTime endDateTime;
            if (!string.IsNullOrEmpty(endDate) && !DateTime.TryParseExact(endDate, formats, CultureInfo.InvariantCulture, DateTimeStyles.None, out endDateTime))
            {
                problem.Errors.Add(new ValidationProblem.ProblemDetails("end-date", "invalid-format", "Bad end-date format"));
            }

            string[] valid_kinds = {"dep", "wdw", "pmt", "fee", "inc", "rev", "adj", "lnd", "lnr", "fcx", "aop","acl", "spl", "sal"};

            // check if transaction kind is a valid code
            if (!string.IsNullOrEmpty(transactionKind))
            {
                // transactionKind is an array with ',' delimiter 
                string[] kinds = transactionKind.Split(',');
                foreach (string kind in kinds)
                {
                    if (!valid_kinds.Contains(kind))
                    {
                        problem.Errors.Add(new ValidationProblem.ProblemDetails("transaction-kind", "unknown-enum", kind + " is not a valid transaction kind"));
                    }
                }
            } 

            if (problem.Errors.Count() > 0)
            {
                return BadRequest(problem);
            } 

            TransactionPagedList<TransactionWithSplits> transactions;

            if (!string.IsNullOrEmpty(startDate) && !string.IsNullOrEmpty(endDate))
            {
                // both strings are not null and valid
                transactions = await _transactionService.getTransactionsAsync(page, pageSize, sortOrder, sortBy, 
                    DateTime.ParseExact(startDate, formats, CultureInfo.InvariantCulture, DateTimeStyles.None).ToUniversalTime(), DateTime.ParseExact(endDate, formats, CultureInfo.InvariantCulture, DateTimeStyles.None).ToUniversalTime(), transactionKind);

            }
            else if (!string.IsNullOrEmpty(startDate) && string.IsNullOrEmpty(endDate))
            {
                // start date is not null and valid
                // end date is null
                transactions = await _transactionService.getTransactionsAsync(page, pageSize, sortOrder, sortBy,
                    DateTime.ParseExact(startDate, formats, CultureInfo.InvariantCulture, DateTimeStyles.None).ToUniversalTime(), DateTime.Now.ToUniversalTime(), transactionKind);
            }
            else if (string.IsNullOrEmpty(startDate) && !string.IsNullOrEmpty(endDate))
            {
                // start date is null
                // end date is not null
                transactions = await _transactionService.getTransactionsAsync(page, pageSize, sortOrder, sortBy,
                    DateTime.MinValue.ToUniversalTime(), DateTime.ParseExact(endDate, formats, CultureInfo.InvariantCulture, DateTimeStyles.None).ToUniversalTime(), transactionKind);
            }
            else
            {
                // both fields are null or empty
                transactions = await _transactionService.getTransactionsAsync(page, pageSize, sortOrder, sortBy,
                    null, null, transactionKind);

            }

            return Ok(transactions);
        }
        

        [HttpPost("import")]
        public async Task<IActionResult> ImportTransactionsAsync(IFormFile csvFile)
        {
            // u req body se nalazi csv fajl 
            if (csvFile == null || csvFile.Length == 0)
            {
                // treba da se vrati odgovor tipa validation problem sa detaljima
                // TODO
                var problem = new ValidationProblem();
                problem.Errors.Add(new ValidationProblem.ProblemDetails(csvFile != null ? csvFile.FileName : "", "invalid-format", "CSV fajl nije poslat ili je duzine 0"));
                return BadRequest(problem);
            }

            // _logger.LogInformation("Evo me u controlleru");

            var value = await _transactionService.importTransactionsFromCSV(csvFile);

            if (value == false)
            {
                return BadRequest("ERROR: Wrong CSV header format!");
            }

            return Ok("File uploaded and data saved.");
        }

        protected IActionResult Index()
        {
            return View();
        }
    }
}
