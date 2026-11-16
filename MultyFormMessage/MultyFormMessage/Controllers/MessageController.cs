using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MultyFormMessage.Filters;
using MultyFormMessage.Models;
using MultyFormMessage.Services;
using System;
using System.Linq;

namespace MultyFormMessage.Controllers
{
    [Authorize]
    public class MessageController : Controller
    {
        private readonly IEmailSender _emailSender;

        public MessageController(IEmailSender emailSender)
        {
            _emailSender = emailSender;
        }

        [HttpGet]
        public IActionResult Index() => View();

        [HttpPost]
        [ValidateAntiForgeryToken]
        [CustomExceptionFilter]
        public IActionResult Send(EmailSendViewModel model)
        {
            if (ModelState.IsValid)
            {
                var recipients = model.ToAddresses.Split(new[] { ',', ';' }, StringSplitOptions.RemoveEmptyEntries)
                                                  .Select(r => r.Trim());

                var message = new Message(recipients, model.Subject, model.Content, model.Attachments);
                _emailSender.SendEmail(message);

                ViewBag.Status = "Лист успішно надіслано!";
                return View("Index");
            }
            return View("Index", model);
        }
    }
}