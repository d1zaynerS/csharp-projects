using Microsoft.AspNetCore.Mvc;
using StoreAndBookingApp.Services;

namespace StoreAndBookingApp.Controllers
{
    public class RoomsController : Controller
    {
        private readonly IBookingService _service;

        public RoomsController(IBookingService service)
        {
            _service = service;
        }

        [HttpGet]
        public IActionResult Index()
        {
            return Json(_service.GetRooms());
        }
    }
}