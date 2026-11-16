using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using UserNotesApp.Data;
using UserNotesApp.Models;

namespace UserNotesApp.Controllers
{
    public class ArticlesController : Controller
    {
        private readonly ApplicationContext _context;
        private readonly UserManager<User> _userManager;

        public ArticlesController(ApplicationContext context, UserManager<User> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        // 1. Головна сторінка — перегляд ВСІХ статей (доступно всім)
        [HttpGet]
        public async Task<IActionResult> Index()
        {
            var articles = await _context.Articles
                .Include(a => a.User)
                .OrderByDescending(a => a.CreatedAt)
                .ToListAsync();

            return View(articles);
        }

        // 2. Створення статті (GET) — доступно тільки авторизованим
        [Authorize]
        [HttpGet]
        public IActionResult Create() => View();

        // 3. Створення статті (POST) — із захистом від null-посилань на користувача
        [Authorize]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Article article)
        {
            var user = await _userManager.GetUserAsync(User);
            if (user == null)
            {
                return RedirectToAction("Login", "Account");
            }

            article.UserId = user.Id;
            article.CreatedAt = DateTime.Now;

            ModelState.Remove("User");
            ModelState.Remove("UserId");

            if (ModelState.IsValid)
            {
                _context.Articles.Add(article);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(article);
        }

        // 4. Сторінка профілю користувача: інфо + список його статей
        [Authorize]
        [HttpGet]
        public async Task<IActionResult> MyProfile()
        {
            var user = await _userManager.GetUserAsync(User);
            if (user == null) return NotFound();

            var userArticles = await _context.Articles
                .Where(a => a.UserId == user.Id)
                .OrderByDescending(a => a.CreatedAt)
                .ToListAsync();

            ViewBag.UserEmail = user.Email;
            ViewBag.UserYear = user.Year;

            return View(userArticles);
        }
    }
}