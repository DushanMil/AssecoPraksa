using AssecoPraksa.Models;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;

namespace AssecoPraksa.Controllers
{
    [EnableCors("MyCORSPolicy")]
    [ApiController]
    [Route("categories")]
    public class CategoriesController : Controller
    {
        [HttpPost("import")]
        public async Task<IActionResult> ImportCategoriesAsync([FromBody] CategoryCSV categoryCSV)
        {
            // u telu se nalazi jedna kategorija
            // pitaj kako se unosi vise kategorija kada je ovde poslata samo jedna
            // takodje da li se unose kategorije iz CSV fajla ili ove iz body dela zahteva

            
            return Ok();
        }

        [HttpGet]
        public async Task<IActionResult> GetCategoriesAsync(
            [FromQuery(Name = "parent-id")] string? parentId = null)
        {
            // vraca categories list, koji kao elemente ima kategoriju
            // TODO
            return Ok();
        }


        protected IActionResult Index()
        {
            return View();
        }
    }
}
