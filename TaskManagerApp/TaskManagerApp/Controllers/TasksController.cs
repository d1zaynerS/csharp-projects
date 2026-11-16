using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using TaskManagerApp.Data;
using TaskManagerApp.Models;

namespace TaskManagerApp.Controllers
{
    [Authorize]
    public class TasksController : Controller
    {
        private readonly ApplicationDbContext _context;

        public TasksController(ApplicationDbContext context)
        {
            _context = context;
        }


        public async Task<IActionResult> Index(string? searchString)
        {
            int userId = int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier)!);

            var notesQuery = _context.Notes.Where(n => n.UserId == userId);

            if (!string.IsNullOrWhiteSpace(searchString))
            {
                notesQuery = notesQuery.Where(n => n.Title.Contains(searchString) || n.Content.Contains(searchString));
            }

            ViewBag.SearchString = searchString;
            var notes = await notesQuery.OrderByDescending(n => n.CreatedAt).ToListAsync();
            return View(notes);
        }


        [HttpPost]
        public async Task<IActionResult> Create(string title, string content)
        {
            if (!string.IsNullOrWhiteSpace(title))
            {
                int userId = int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier)!);

                var note = new UserNote
                {
                    Title = title,
                    Content = content,
                    UserId = userId,
                    CreatedAt = DateTime.Now
                };

                _context.Notes.Add(note);
                await _context.SaveChangesAsync();
            }
            return RedirectToAction("Index");
        }


        [HttpPost]
        public async Task<IActionResult> Delete(int id)
        {
            int userId = int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier)!);

            var note = await _context.Notes.FirstOrDefaultAsync(n => n.Id == id && n.UserId == userId);
            if (note != null)
            {
                _context.Notes.Remove(note);
                await _context.SaveChangesAsync();
            }
            return RedirectToAction("Index");
        }
    }
}