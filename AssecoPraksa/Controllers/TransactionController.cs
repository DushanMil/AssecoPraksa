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
        // treba mi neki servis

        // transaction/{transactionId}/split
        [HttpPost("{transactionId}/split")]
        public async Task<IActionResult> SplitTransactionAsync([FromRoute] string transactionId)
        {
            // ovo treba da ima neki asinhroni poziv
            // u tellu zahteva postoji splits - na koje delove treba podeliti transakciju
            // split-transaction-command, from body, own model, TODO
            // TODO
            return Ok();
        }

        // transaction/{transactionId}/categorize
        [HttpPost("{transactionId}/categorize")]
        public async Task<IActionResult> CategorizeTransactionAsync([FromRoute] string transactionId)
        {
            // ovo treba da ima neki asinhroni poziv
            // transaction categorize command, from body, model, TODO
            // TODO
            return Ok();
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
