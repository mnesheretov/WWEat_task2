using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using System.Diagnostics;
using WWEat.Models;

namespace WWEat.Controllers
{
    public class HomeController : Controller
    {
        private readonly ApplicationDbContext db;

        public HomeController(ApplicationDbContext context)
        {
            db = context;
        }

        public IActionResult Index()
        {
            ViewBag.Dishes = new SelectList(db.Dishes, "Id", "Name");
            return View();
        }

        [HttpPost]
        public ActionResult AddDish(string dishName)
        {
            if (db.Dishes.Any(d => d.Name == dishName))
            {
                return Json(new { success = false, message = "Это блюдо уже кто-то когда-то ел" });
            }

            db.Dishes.Add(new Dish { Name = dishName });
            db.SaveChanges();
            return Json(new { success = true });
        }

        [HttpPost]
        public ActionResult AddMeal(string userName, string userEmail, int dishId)
        {
            if (string.IsNullOrEmpty(userName) || string.IsNullOrEmpty(userEmail))
            {
                return Json(new { success = false, message = "Не все обязательные поля заполнены" });
            }

            var user = db.Users.FirstOrDefault(u => u.Name == userName && u.Email == userEmail);
            bool isFirstTime = false;

            if (user == null)
            {
                user = new User { Name = userName, Email = userEmail };
                db.Users.Add(user);
                db.SaveChanges();
                Console.WriteLine("Пользователь добавлен: " + userName);
                isFirstTime = true;
            }

            var meal = new Meal { UserId = user.Id, DishId = dishId, Timestamp = DateTime.Now };
            db.Meals.Add(meal);
            db.SaveChanges();
            Console.WriteLine("Meal добавлен для пользователя: " + userName);

            var totalMealsToday = db.Meals.Count(m => m.DishId == dishId && m.Timestamp.Date == DateTime.Now.Date);
            var userMeals = db.Meals.Count(m => m.UserId == user.Id && m.DishId == dishId);

            return Json(new
            {
                success = true,
                isFirstTime,
                userName,
                dishName = db.Dishes.Find(dishId).Name,
                totalMealsToday,
                userMeals
            });
        }

        public IActionResult Feed()
        {
            var feed = db.Meals.OrderByDescending(m => m.Timestamp).Take(20).Select(m => new
            {
                Time = m.Timestamp.ToString("HH:mm dd/MM/yyyy"),
                UserName = m.User.Name,
                UserEmail = m.User.Email,
                DishName = m.Dish.Name,
            }).ToList();


            return View(feed);
        }
    }
}