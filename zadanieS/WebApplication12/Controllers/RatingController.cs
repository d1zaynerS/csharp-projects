using Microsoft.AspNetCore.Mvc;
using WebApplication12.Models;

namespace WebApplication12.Controllers
{
    public class RatingController : Controller
    {
        public IActionResult Index()
        {
            var products = new List<ProductRating>
            {
                new ProductRating { Name = "Смартфон", Rating = 5 },
                new ProductRating { Name = "Ноутбук", Rating = 4 },
                new ProductRating { Name = "Навушники", Rating = 3 },
                new ProductRating { Name = "Мишка", Rating = 1 }
            };

            return View(products);
        }
    }
}