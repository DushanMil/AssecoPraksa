using AssecoPraksa.Models;
using AssecoPraksa.Services;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory.Database;

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
            // get categories with set parentId
            // if parentId not set get categories with parentId = null
            // no need to check if parentId is valid
            // Console.WriteLine(parentId);

            var problem = new ValidationProblem();

            var categories = await _categoryService.getCategoryList(parentId);
            if (categories == null)
            {
                // ovo je jedini moguci problem
                // zato ovde moze da se vrati odmah
                problem.Errors.Add(new ValidationProblem.ProblemDetails("m", "Category code doesn't exist", "Code " + parentId + " doesn't exist"));
                return BadRequest(problem);
            }

            return Ok(categories);
        }


        protected IActionResult Index()
        {
            return View();
        }
    }
}
