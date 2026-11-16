using Microsoft.AspNetCore.Mvc;
using BlogAndNotesApp.Models;

namespace BlogAndNotesApp.Controllers
{
    public class NotesController : Controller
    {
        private static readonly List<Note> _notes = new()
        {
            new Note { Id = 1, Title = "Купити продукти", Content = "Молоко, хліб, сир", Category = "Покупки" },
            new Note { Id = 2, Title = "Завдання MVC", Content = "Зробити лабу по Cookie та AutoMapper", Category = "Навчання" },
            new Note { Id = 3, Title = "Проект Блог", Content = "Додати нову тему", Category = "Навчання" }
        };

        public IActionResult Index(string? category)
        {

            if (string.IsNullOrEmpty(category))
            {
                category = Request.Cookies["LastNoteCategory"] ?? "Всі";
            }
            else
            {

                Response.Cookies.Append("LastNoteCategory", category, new CookieOptions { Expires = DateTime.Now.AddDays(7) });
            }

            ViewBag.SelectedCategory = category;
            ViewBag.Categories = new List<string> { "Всі", "Покупки", "Навчання" };

            var filteredNotes = category == "Всі"
                ? _notes
                : _notes.Where(n => n.Category == category).ToList();

            return View(filteredNotes);
        }
    }
}