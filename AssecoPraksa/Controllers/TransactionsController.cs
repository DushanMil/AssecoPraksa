using AssecoPraksa.Models;
using AssecoPraksa.Services;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;

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
            // TODO
            return Ok();
        }



        // Helper function for parsing a csv file
        

        [HttpPost("import")]
        public async Task<IActionResult> ImportTransactionsAsync([FromBody] IFormFile csvFile)
        {
            // u req body se nalazi csv fajl 
            if (csvFile == null || csvFile.Length == 0)
            {
                // treba da se vrati odgovor tipa validation problem sa detaljima
                // TODO
                return BadRequest("No file uploaded!");
            }

            // ne znam sta ce mi povratna vrednost ovoga ali dobro
            var value = await _transactionService.importTransactionsFromCSV(csvFile);

            return Ok("File uploaded and data saved.");
        }

        protected IActionResult Index()
        {
            return View();
        }
    }
}
