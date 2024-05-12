using HarmonyAPI.Data;
using HarmonyAPI.Model;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;

namespace HarmonyAPI.Controllers
{
    [Route("[controller]/{id?}")]
    public class UserController : Controller
    {
        ApplicationContext db;
        private readonly ILogger<UserController> logger;

        public UserController(ILogger<UserController> logger, ApplicationContext context)
        {
            this.logger = logger;
            db = context;
        }

        /// <summary>
        /// Создаем пользователей и заодно и склады
        /// Логика по созданию пользователя в классе User
        /// Создаем соль и шифруем пароль
        /// Храним соль и зашифрованный пароль в базе
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public async Task<ActionResult> Index()
        {

            var users = await db.Users.ToListAsync();
            var categ = db.Categories.ToList();
            var manuf = db.Manufacturers.ToList();
            var prod = db.Products.ToList();
            if (prod.Count == 0)
            {
                var p1 = new Product { Name = "Аспирин", CategoryId = categ[0].Id, ManufacturerId = manuf[0].Id, Price = 200 };
                var p2 = new Product { Name = "Анальгин", CategoryId = categ[1].Id, ManufacturerId = manuf[1].Id, Price = 100 };
                db.Add(p1);
                db.Add(p2);
                db.SaveChanges();
            }

            var st = db.Stocks.ToList();
            if (st.Count == 0)
            {
                var det = db.StocksDetail.ToList();
                var s1 = new Stock { DetailId = det[0].Id, ProductId = prod[0].Id, Quantity = 10 };
                var s2 = new Stock { DetailId = det[1].Id, ProductId = prod[1].Id, Quantity = 20 };
                var s3 = new Stock { DetailId = det[2].Id, ProductId = prod[0].Id, Quantity = 30 };
                db.Add(s1);
                db.Add(s2);
                db.Add(s3);
                db.SaveChanges();

            }


            if (users.Count == 0)
            {
                var u1 = new User("Mechta1", "testpass1");
                u1.StockDetailId = st[0].DetailId;

                var u2 = new User("Mechta2", "testpass2");
                u2.StockDetailId = st[1].DetailId;
                var u3 = new User("Mechta3", "testpass3");
                u3.StockDetailId = st[2].DetailId;
                users.Add(u1);
                users.Add(u2);
                users.Add(u3);
                await db.Users.AddRangeAsync(users);
                await db.SaveChangesAsync();
            }
            return Ok();
        }

            /// <summary>
            /// Если пользователь у нас есть в базе
            /// Метод из класса User скачиваем соль и хешированный пароль из базы 
            /// и сравниваем их
            /// </summary>
            /// <param name="userDTO">Возвращаем пользователю ID его склада 
            /// Все дальнейшие запросы пользователь делает по этому ID
            /// Также отдаем ему токен для авторизации
            /// </param>
            /// <returns></returns>
            [HttpPost]
        public async Task<ActionResult> Auth([FromBody] UserDTO userDTO)
        {
            var user = await db.Users.Where(x => x.Login == userDTO.Login).FirstOrDefaultAsync();
            if (user == null)
            {
                return BadRequest();
            }


            var claims = new List<Claim> { new Claim(ClaimTypes.Name, user.Id.ToString()) };
            var jwt = new JwtSecurityToken(
                    issuer: AuthOptions.ISSUER,
                    audience: AuthOptions.AUDIENCE,
                    claims: claims,
                    expires: DateTime.UtcNow.Add(TimeSpan.FromMinutes(600)),
                    signingCredentials: new SigningCredentials(AuthOptions.GetSymmetricSecurityKey(), SecurityAlgorithms.HmacSha256));

            var token = new JwtSecurityTokenHandler().WriteToken(jwt);

            return Json(new AuthData { Id = user.StockDetailId, Token = token });


        }


    }
}
