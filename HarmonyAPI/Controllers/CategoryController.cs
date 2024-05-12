using HarmonyAPI.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace HarmonyAPI.Controllers
{
    [Route("[controller]/{id?}")]
    public class CategoryController : Controller
    {
        ApplicationContext db;
        private readonly ILogger<CategoryController> logger;

        public CategoryController(ILogger<CategoryController> logger, ApplicationContext context)
        {
            this.logger = logger;
            db = context;
        }
        [HttpGet]
        public async Task<ActionResult> Index()
        {
            var categories =await db.Categories.ToListAsync();

            return Json(categories);
        }

    }

}
