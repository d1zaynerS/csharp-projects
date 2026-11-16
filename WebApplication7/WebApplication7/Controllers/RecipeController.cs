using Microsoft.AspNetCore.Mvc;
using StoreAndBookingApp.Models;

namespace StoreAndBookingApp.Controllers
{
    public class RecipeController : Controller
    {
        private static readonly List<Recipe> Recipes = new()
        {
            new Recipe { Id = 1, Name = "Паста", Category = "Основне", Ingredients = new() { "Макарони", "Сир" } },
            new Recipe { Id = 2, Name = "Омлет", Category = "Сніданок", Ingredients = new() { "Яйця", "Молоко" } },
            new Recipe { Id = 3, Name = "Борщ", Category = "Основне", Ingredients = new() { "Буряк", "М'ясо" } }
        };

        private static readonly Random Rnd = new();

        [HttpGet]
        public IActionResult Random()
        {
            return Json(Recipes[Rnd.Next(Recipes.Count)]);
        }

        [HttpGet]
        public IActionResult Ingredient(string ingredient)
        {
            var filtered = Recipes.Where(r => r.Ingredients.Any(i => i.Equals(ingredient, StringComparison.OrdinalIgnoreCase))).ToList();
            if (!filtered.Any()) return NotFound("Рецептів не знайдено.");
            return Json(filtered[Rnd.Next(filtered.Count)]);
        }

        [HttpGet]
        public IActionResult Category(string category)
        {
            var filtered = Recipes.Where(r => r.Category.Equals(category, StringComparison.OrdinalIgnoreCase)).ToList();
            if (!filtered.Any()) return NotFound("Категорію не знайдено.");
            return Json(filtered[Rnd.Next(filtered.Count)]);
        }
    }
}