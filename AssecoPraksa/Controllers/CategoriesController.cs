using AssecoPraksa.Models;
using AssecoPraksa.Services;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;

namespace AssecoPraksa.Controllers
{
    [EnableCors("MyCORSPolicy")]
    [ApiController]
    [Route("categories")]
    public class CategoriesController : Controller
    {
        ICategoryService _categoryService;

        public CategoriesController(ICategoryService categoryService)
        {
            _categoryService = categoryService;
        }

        [HttpPost("import")]
        public async Task<IActionResult> ImportCategoriesAsync(IFormFile csvFile)
        {
            // unos kategorija iz csv fajla
            if (csvFile == null || csvFile.Length == 0)
            {
                // treba da se vrati odgovor tipa validation problem sa detaljima
                // TODO
                var problem = new ValidationProblem();
                problem.Errors.Add(new ValidationProblem.ProblemDetails(csvFile != null ? csvFile.FileName : "", "invalid-format", "CSV fajl nije poslat ili je duzine 0"));
                return BadRequest(problem);
            }

            var value = await _categoryService.importCategoriesFromCSV(csvFile);

            if (value == false)
            {
                return BadRequest("ERROR: Wrong CSV header format!");
            }

            return Ok("File uploaded and data saved.");
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
