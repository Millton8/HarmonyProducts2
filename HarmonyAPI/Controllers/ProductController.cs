using HarmonyAPI.Data;
using HarmonyAPI.Model;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace HarmonyAPI.Controllers
{

    [Route("[controller]/{id?}")]
    public class ProductController : Controller
    {
        ApplicationContext db;
        private readonly ILogger<ProductController> logger;

        public ProductController(ILogger<ProductController> logger, ApplicationContext context)
        {
            this.logger = logger;
            db = context;
        }
        
        [HttpGet]
        public async Task<ActionResult> Index(Guid id)
        {
            var products= await db.Products.Include(item=>item.Manufacturer).Include(item=>item.Category).ToListAsync();

            return Json(products);
        }

        [HttpPost]
        public async Task<ActionResult> Add([FromBody] Product product)
        {
            if (product == null)
                return BadRequest();
            var category=await db.Categories.SingleOrDefaultAsync(x=>x.Id==product.Id);
            product.Category = category;

            await db.Products.AddAsync(product);
            await db.SaveChangesAsync();
            var response = await db.Products.Include(item => item.Manufacturer).Include(item => item.Category).ToListAsync();

            return Json(response);
        }

        [HttpPut]
        public async Task<ActionResult> EditProduct([FromBody] Product product)
        {

            var myProduct = await db.Products.Where(item => item.Id == product.Id).FirstOrDefaultAsync();
            if (myProduct == null)
                return StatusCode(490);

            db.Products.Entry(myProduct).CurrentValues.SetValues(product);
            await db.SaveChangesAsync();


            return Ok();
        }

        [HttpDelete]
        public async Task<ActionResult> DeleteProduct(Guid id)
        {

            var myProduct = await db.Products.Where(item => item.Id == id).FirstOrDefaultAsync();
            if (myProduct == null)
                return StatusCode(490);

            db.Remove(myProduct);
            await db.SaveChangesAsync();


            return Ok();
        }
    }
}
