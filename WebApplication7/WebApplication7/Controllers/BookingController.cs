using Microsoft.AspNetCore.Mvc;
using StoreAndBookingApp.Models;
using StoreAndBookingApp.Services;

namespace StoreAndBookingApp.Controllers
{
    public class BookingController : Controller
    {
        private readonly IBookingService _service;
        public BookingController(IBookingService service) => _service = service;

        [HttpGet]
        public IActionResult Index() => Json(_service.GetBookings());

        [HttpPost]
        public IActionResult Create(Booking booking)
        {
            bool success = _service.AddBooking(booking);
            if (!success) return BadRequest("Обраний час зайнятий або вказаний у минулому.");
            return RedirectToAction("Index");
        }

        [HttpPost]
        public IActionResult Cancel(int id)
        {
            _service.CancelBooking(id);
            return RedirectToAction("Index");
        }
    }
}