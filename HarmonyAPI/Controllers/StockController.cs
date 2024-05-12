using HarmonyAPI.Data;
using HarmonyAPI.Model;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace HarmonyAPI.Controllers
{
    [Route("[controller]/{id?}")]
    public class StockController : Controller
    {
        ApplicationContext db;
        private readonly ILogger<StockController> logger;

        public StockController(ILogger<StockController> logger, ApplicationContext context)
        {
            this.logger = logger;
            db = context;
        }
        /// <summary>
        /// Пока единственный метод с авторизацией
        /// Если есть ID значит это обычный пользователь.
        /// Отдаем ему информацию по его складу
        /// Если нет ID значит это администратор
        /// Отдаем ему информацию по всем складам
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet]
        [Authorize]
        public async Task<ActionResult> Index(Guid id)
        {
            if (id == Guid.Empty)
            {
                var stocks = await db.Stocks.Include(x=>x.Detail)
                                            .Include(x => x.Product).ThenInclude(x => x.Manufacturer)
                                            .Include(x => x.Product).ThenInclude(x => x.Category)
                                            .ToListAsync();
                return Json(stocks);
            }

            var stock = await (from s in db.Stocks
                               where s.DetailId == id && s.Quantity>0
                               join p in db.Products on s.ProductId equals p.Id
                               join m in db.Manufacturers on p.ManufacturerId equals m.Id into manuf
                               from m in manuf.DefaultIfEmpty()
                               join c in db.Categories on p.CategoryId equals c.Id
                               select new ProductDTO
                               {
                                   Id = p.Id,
                                   Name = p.Name,
                                   ManufacturerName = m != null ? m.Name : "Не указан",
                                   CategoryName = c.Name,
                                   Price = p.Price,
                                   ConditionPrice = p.ConditionPrice,
                                   Bonus = p.Bonus,
                                   IsRecept = p.isRecept,
                                   Images = p.Images,
                                   RetailCount = p.RetailCount,
                                   Description = p.Description,
                                   Quantity = s.Quantity
                               }).ToListAsync();


            return Json(stock);
        }
    }
}
