using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using StoreAndBookingApp.Data;
using StoreAndBookingApp.Models;

namespace StoreAndBookingApp.Controllers
{
    public class ProductController : Controller
    {
        private readonly AppDbContext _db;
        public ProductController(AppDbContext db) => _db = db;

        [HttpGet]
        public async Task<IActionResult> Index() => Json(await _db.Products.ToListAsync());

        [HttpGet]
        public IActionResult Create() => View();

        [HttpPost]
        public async Task<IActionResult> Create(Product product)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);
            _db.Products.Add(product);
            await _db.SaveChangesAsync();
            return Json(product);
        }

        [HttpGet]
        public async Task<IActionResult> Search(string keyword)
        {
            if (string.IsNullOrWhiteSpace(keyword)) return Json(new List<Product>());

            var lowerKeyword = keyword.ToLower();
            var items = await _db.Products
                .Where(p => p.Name.ToLower().Contains(lowerKeyword) ||
                            p.Description.ToLower().Contains(lowerKeyword))
                .ToListAsync();

            return Json(items);
        }

        [HttpGet]
        public async Task<IActionResult> Details(int id)
        {
            var product = await _db.Products.FindAsync(id);
            return product == null ? NotFound() : Json(product);
        }

        [HttpPost]
        public async Task<IActionResult> Delete(int id)
        {
            var product = await _db.Products.FindAsync(id);
            if (product == null) return NotFound();
            _db.Products.Remove(product);
            await _db.SaveChangesAsync();
            return Json(product);
        }
    }
}