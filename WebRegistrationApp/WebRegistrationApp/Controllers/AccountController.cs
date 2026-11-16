using Microsoft.AspNetCore.Mvc;
using WebRegistrationApp.Data;
using WebRegistrationApp.Models;

namespace WebRegistrationApp.Controllers
{
    public class AccountController : Controller
    {
        private readonly ApplicationDbContext _context;

        public AccountController(ApplicationDbContext context)
        {
            _context = context;
        }

        [HttpGet]
        public IActionResult Register()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Register(RegisterViewModel model)
        {

            if (ModelState.IsValid)
            {
                var user = new User
                {
                    FirstName = model.FirstName,
                    LastName = model.LastName,
                    Username = model.Username,
                    Email = model.Email,
                    Password = model.Password,
                    Age = model.Age,
                    PhoneNumber = model.PhoneNumber,
                    CreditCardNumber = model.CreditCardNumber,
                    Website = model.Website
                };

                _context.Users.Add(user);
                await _context.SaveChangesAsync();

                return Content("Реєстрація успішна! Користувача збережено в базі даних.");
            }


            return View(model);
        }


        [AcceptVerbs("Get", "Post")]
        public IActionResult VerifyUsername(string username)
        {
            bool userExists = _context.Users.Any(u => u.Username == username);
            if (userExists)
            {
                return Json($"Ім'я користувача '{username}' вже зайняте.");
            }
            return Json(true);
        }
    }
}