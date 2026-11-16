using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using UserNotesApp.Data;
using UserNotesApp.Models;

namespace UserNotesApp.Controllers
{
    [Authorize]
    public class FriendsController : Controller
    {
        private readonly ApplicationContext _context;
        private readonly UserManager<User> _userManager;

        public FriendsController(ApplicationContext context, UserManager<User> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        public async Task<IActionResult> Index()
        {
            var userId = _userManager.GetUserId(User);

            var friendships = await _context.Friendships
                .Include(f => f.Sender)
                .Include(f => f.Receiver)
                .Where(f => f.SenderId == userId || f.ReceiverId == userId)
                .ToListAsync();

            var allUsers = await _userManager.Users.Where(u => u.Id != userId).ToListAsync();
            ViewBag.AllUsers = allUsers;

            return View(friendships);
        }

        [HttpPost]
        public async Task<IActionResult> SendRequest(string receiverId)
        {
            var senderId = _userManager.GetUserId(User);
            if (senderId == receiverId || string.IsNullOrEmpty(receiverId)) 
                return RedirectToAction(nameof(Index));

            var receiverExists = await _userManager.FindByIdAsync(receiverId);
            if (receiverExists == null) 
                return RedirectToAction(nameof(Index));

            var exists = await _context.Friendships
                .AnyAsync(f => (f.SenderId == senderId && f.ReceiverId == receiverId) || (f.SenderId == receiverId && f.ReceiverId == senderId));

            if (!exists)
            {
                var friendship = new Friendship { SenderId = senderId!, ReceiverId = receiverId, IsAccepted = false };
                _context.Friendships.Add(friendship);
                await _context.SaveChangesAsync();
            }

            return RedirectToAction(nameof(Index));
        }

        [HttpPost]
        public async Task<IActionResult> AcceptRequest(int id)
        {
            var userId = _userManager.GetUserId(User);
            var friendship = await _context.Friendships.FindAsync(id);

            if (friendship != null && friendship.ReceiverId == userId)
            {
                friendship.IsAccepted = true;
                await _context.SaveChangesAsync();
            }

            return RedirectToAction(nameof(Index));
        }

        [HttpPost]
        public async Task<IActionResult> RemoveFriend(int id)
        {
            var userId = _userManager.GetUserId(User);
            var friendship = await _context.Friendships.FindAsync(id);

            if (friendship != null && (friendship.SenderId == userId || friendship.ReceiverId == userId))
            {
                _context.Friendships.Remove(friendship);
                await _context.SaveChangesAsync();
            }

            return RedirectToAction(nameof(Index));
        }
    }
}