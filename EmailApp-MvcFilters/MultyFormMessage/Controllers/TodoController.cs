using Microsoft.AspNetCore.Mvc;
using MultyFormMessage.Data;
using MultyFormMessage.Filters;
using MultyFormMessage.Models;
using System.Linq;

namespace MultyFormMessage.Controllers
{
    public class TodoController : Controller
    {
        private readonly ApplicationContext _context;

        public TodoController(ApplicationContext context)
        {
            _context = context;
        }

        public IActionResult Index() => View(_context.TodoItems.ToList());

        [HttpPost]
        [TimestampActionFilter]
        public IActionResult Create(TodoItem item)
        {
            if (ModelState.IsValid)
            {
                _context.TodoItems.Add(item);
                _context.SaveChanges();
            }
            return RedirectToAction("Index");
        }
    }
}