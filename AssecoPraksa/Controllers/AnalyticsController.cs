using AssecoPraksa.Models;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;

namespace AssecoPraksa.Controllers
{
    [EnableCors("MyCORSPolicy")]
    [ApiController]
    [Route("spending-analytics")]
    public class AnalyticsController : Controller
    {
        [HttpGet]
        public async Task<IActionResult> GetSpendingAnalyticsAsync(
            [FromQuery] string? catcode = null,
            [FromQuery(Name = "start-date")] string? startDate = null,
            [FromQuery(Name = "end-date")] string? endDate = null,
            [FromQuery] Direction? direction = null)
        {
            // vraca spending by category, listu potrosnje po kategorijama
            // TODO
            return Ok();
        }

        protected IActionResult Index()
        {
            return View();
        }
    }
}
