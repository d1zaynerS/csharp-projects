using Microsoft.AspNetCore.Mvc;
using BlogAndNotesApp.Models;

namespace BlogAndNotesApp.Controllers
{
    public class BlogController : Controller
    {
        private static readonly List<News> _newsList = new()
        {
            new News { Id = 1, Title = "Перший допис", Content = "Ласкаво просимо до мого особистого блогу!", Date = DateTime.Now.AddDays(-2) },
            new News { Id = 2, Title = "Оновлення ASP.NET Core", Content = "Сьогодні вивчаємо роботу з Cookie в MVC.", Date = DateTime.Now.AddDays(-1) }
        };

        public IActionResult Index()
        {
            ViewBag.Theme = Request.Cookies["BlogTheme"] ?? "light";
            return View(_newsList);
        }

        [HttpGet]
        public IActionResult Settings()
        {
            ViewBag.Theme = Request.Cookies["BlogTheme"] ?? "light";
            return View();
        }

        [HttpPost]
        public IActionResult Settings(string theme)
        {
            CookieOptions options = new CookieOptions
            {
                Expires = DateTime.Now.AddDays(30)
            };

            Response.Cookies.Append("BlogTheme", theme, options);
            return RedirectToAction("Index");
        }
    }
}