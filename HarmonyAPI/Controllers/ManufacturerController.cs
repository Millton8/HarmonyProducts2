using HarmonyAPI.Data;
using HarmonyAPI.Model;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace HarmonyAPI.Controllers
{
    [Route("[controller]/{id?}")]
    public class ManufacturerController : Controller
    {
        ApplicationContext db;
        private readonly ILogger<ManufacturerController> logger;

        public ManufacturerController(ILogger<ManufacturerController> logger, ApplicationContext context)
        {
            this.logger = logger;
            db = context;
        }
        [HttpGet]
        public async Task<ActionResult> Index()
        {

            var manufacturers = await db.Manufacturers.ToListAsync();

            return Json(manufacturers);
        }

        [HttpPost]
        public async Task<ActionResult> AddManufacturer([FromBody] string name)
        {
            
            if (db.Manufacturers.Any(item=>item.Name==name))
                return StatusCode(490);

            var manufacturer = new Manufacturer { Name = name };
            await db.Manufacturers.AddAsync(manufacturer);
            await db.SaveChangesAsync();
            var response = await db.Manufacturers.ToListAsync();

            return Json(response);
        }

        [HttpPut]
        public async Task<ActionResult> EditManufacturer([FromBody] Manufacturer manufacturer)
        {
                        
            var myManufacturer=await db.Manufacturers.Where(item=>item.Id== manufacturer.Id).FirstOrDefaultAsync();
            if (myManufacturer==null)
                return StatusCode(490);
            myManufacturer.Name = manufacturer.Name;
            await db.SaveChangesAsync();


            return Ok();
        }

        [HttpDelete]
        public async Task<ActionResult> DeleteManufacturer(Guid id)
        {
           
            var myManufacturer = await db.Manufacturers.Where(item => item.Id == id).FirstOrDefaultAsync();
            if (myManufacturer == null)
                return StatusCode(490);

            db.Remove(myManufacturer);
            await db.SaveChangesAsync();


            return Ok();
        }
    }
}
