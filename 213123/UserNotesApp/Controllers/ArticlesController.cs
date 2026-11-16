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

        // 1. Головна сторінка — фільтрація згідно з ТЗ
        [HttpGet]
        public async Task<IActionResult> Index()
        {
            var currentUserId = _userManager.GetUserId(User);
            var friendIds = new List<string>();

            if (currentUserId != null)
            {
                friendIds = await _context.Friendships
                    .Where(f => (f.SenderId == currentUserId || f.ReceiverId == currentUserId) && f.IsAccepted)
                    .Select(f => f.SenderId == currentUserId ? f.ReceiverId : f.SenderId)
                    .ToListAsync();
            }

            var articles = await _context.Articles
                .Include(a => a.User)
                .Where(a => !a.IsHiddenByModerator || (currentUserId != null && a.UserId == currentUserId))
                .Where(a => 
                    a.Visibility == PostVisibility.Public ||
                    (currentUserId != null && a.UserId == currentUserId) ||
                    (currentUserId != null && a.Visibility == PostVisibility.Friends && friendIds.Contains(a.UserId))
                )
                .OrderByDescending(a => a.CreatedAt)
                .ToListAsync();

            return View(articles);
        }

        [Authorize]
        [HttpGet]
        public IActionResult Create() => View();

        [Authorize]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Article article)
        {
            var user = await _userManager.GetUserAsync(User);
            if (user == null) return RedirectToAction("Login", "Account");

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

        // Модератор: скрытие/показ чужих постов
        [Authorize(Roles = "Moderator,Admin")]
        [HttpPost]
        public async Task<IActionResult> ToggleHide(int id)
        {
            var article = await _context.Articles.FindAsync(id);
            if (article != null)
            {
                article.IsHiddenByModerator = !article.IsHiddenByModerator;
                await _context.SaveChangesAsync();
            }
            return RedirectToAction(nameof(Index));
        }

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