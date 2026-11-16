using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Blog.Models;

namespace Blog.Controllers
{
    [Authorize(Roles = "Admin")]
    public class SubscriptionController : Controller
    {
        private readonly ApplicationContext _db;

        public SubscriptionController(ApplicationContext db)
        {
            _db = db;
        }

        [HttpGet]
        public async Task<IActionResult> Index()
        {
            var subscribers = await _db.Subscribers
                .OrderByDescending(s => s.SubscribedAt)
                .ToListAsync();

            return View(subscribers);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Delete(int id)
        {
            var subscriber = await _db.Subscribers.FindAsync(id);
            if (subscriber != null)
            {
                _db.Subscribers.Remove(subscriber);
                await _db.SaveChangesAsync();
            }

            return RedirectToAction(nameof(Index));
        }

        [HttpPost]
        public async Task<IActionResult> PrepareNewsletter(List<string> selectedEmails)
        {
            ViewBag.SelectedEmails = selectedEmails;
            return View("SendEmail");
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> SendEmail(List<string> selectedEmails, string subject, string body)
        {
            if (selectedEmails == null || !selectedEmails.Any())
            {
                selectedEmails = await _db.Subscribers
                    .Select(s => s.Email)
                    .ToListAsync();
            }

            TempData["SuccessMessage"] = $"Лист успішно надіслано {selectedEmails.Count} передплатникам!";

            return RedirectToAction(nameof(Index));
        }
    }
}