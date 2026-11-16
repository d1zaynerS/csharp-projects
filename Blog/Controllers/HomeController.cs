using System;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Blog.Models;
using Blog.ViewModels;

namespace Blog.Controllers
{
    public class HomeController : Controller
    {
        private readonly ApplicationContext _db;

        public HomeController(ApplicationContext db)
        {
            _db = db;
        }

        public async Task<IActionResult> Index(int page = 1)
        {
            int pageSize = 6;
            var query = _db.Publications
                .Include(p => p.Categories)
                .OrderByDescending(p => p.Id);

            int count = await query.CountAsync();
            var items = await query
                .Skip((page - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync();

            var viewModel = new IndexViewModel<Publication>
            {
                Items = items,
                PageViewModel = new PageViewModel(count, page, pageSize)
            };

            return View(viewModel);
        }

        public async Task<IActionResult> Details(Guid id)
        {
            var publication = await _db.Publications
                .Include(p => p.Categories)
                .FirstOrDefaultAsync(p => p.Id == id);

            if (publication == null)
            {
                return NotFound();
            }

            publication.TotalViews++;
            await _db.SaveChangesAsync();

            Random rand = new Random();
            int num1 = rand.Next(1, 10);
            int num2 = rand.Next(1, 10);

            HttpContext.Session.SetInt32("CaptchaResult", num1 + num2);
            ViewBag.CaptchaQuestion = $"{num1} + {num2} = ?";

            ViewBag.Comments = await _db.Comments
                .Where(c => c.PublicationId == id)
                .OrderByDescending(c => c.CreatedAt)
                .ToListAsync();

            return View(publication);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> AddComment(Guid publicationId, string authorName, string? email, string message, int captchaInput)
        {
            int? captchaResult = HttpContext.Session.GetInt32("CaptchaResult");

            if (captchaResult == null || captchaInput != captchaResult)
            {
                TempData["Error"] = "Невірно розв'язано капчу!";
                return RedirectToAction(nameof(Details), new { id = publicationId });
            }

            if (string.IsNullOrWhiteSpace(authorName) || string.IsNullOrWhiteSpace(message))
            {
                TempData["Error"] = "Будь ласка, заповніть обов'язкові поля!";
                return RedirectToAction(nameof(Details), new { id = publicationId });
            }

            var comment = new Comment
            {
                PublicationId = publicationId,
                AuthorName = authorName,
                Email = email,
                Message = message,
                CreatedAt = DateTime.Now
            };

            _db.Comments.Add(comment);
            await _db.SaveChangesAsync();

            return RedirectToAction(nameof(Details), new { id = publicationId });
        }
    }
}