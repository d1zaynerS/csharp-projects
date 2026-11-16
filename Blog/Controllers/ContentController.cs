using System;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Blog.Interfaces;
using Blog.Models;
using Blog.ViewModels;

namespace Blog.Controllers
{
    [Authorize(Roles = "Admin")]
    public class ContentController : Controller
    {
        private readonly IPublication _publicationRepository;
        private readonly ICategory _categoryRepository;
        private readonly IWebHostEnvironment _appEnvironment;

        public ContentController(
            IPublication publicationRepository,
            ICategory categoryRepository,
            IWebHostEnvironment appEnvironment)
        {
            _publicationRepository = publicationRepository;
            _categoryRepository = categoryRepository;
            _appEnvironment = appEnvironment;
        }

        [HttpGet]
        public async Task<IActionResult> Index()
        {
            var publications = await _publicationRepository.GetAllPublicationsAsync();
            var categories = await _categoryRepository.GetAllCategoriesAsync();

            var viewModel = new ContentViewModel
            {
                Publications = publications,
                Categories = categories
            };

            return View(viewModel);
        }

        [HttpGet]
        [Route("create-publication")]
        public async Task<IActionResult> CreatePublication()
        {
            var categories = await _categoryRepository.GetAllCategoriesAsync();
            var viewModel = new PublicationViewModel
            {
                SelectListCategories = categories.Select(c => new SelectListItem
                {
                    Value = c.Id.ToString(),
                    Text = c.Name
                }).ToList()
            };

            return View(viewModel);
        }

        [HttpPost]
        [Route("create-publication")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> CreatePublication(PublicationViewModel model, IFormFile? image)
        {
            if (ModelState.IsValid)
            {
                string? imagePath = null;

                if (image != null && image.Length > 0)
                {
                    string uploadsFolder = Path.Combine(_appEnvironment.WebRootPath, "publicationImages");

                    if (!Directory.Exists(uploadsFolder))
                    {
                        Directory.CreateDirectory(uploadsFolder);
                    }

                    string uniqueFileName = Guid.NewGuid().ToString() + "_" + Path.GetFileName(image.FileName);
                    string filePath = Path.Combine(uploadsFolder, uniqueFileName);

                    using (var fileStream = new FileStream(filePath, FileMode.Create))
                    {
                        await image.CopyToAsync(fileStream);
                    }

                    imagePath = "/publicationImages/" + uniqueFileName;
                }

                var publication = new Publication
                {
                    Id = Guid.NewGuid(),
                    Title = model.Title,
                    Description = model.Description,
                    SeoDescription = model.SeoDescription,
                    Keywords = model.Keywords,
                    Content = model.Content,
                    ImagePath = imagePath,
                    Image = imagePath,
                    FullImageName = imagePath
                };

                await _publicationRepository.AddPublicationAsync(publication);
                return RedirectToAction("Index");
            }

            var categories = await _categoryRepository.GetAllCategoriesAsync();
            model.SelectListCategories = categories.Select(c => new SelectListItem
            {
                Value = c.Id.ToString(),
                Text = c.Name
            }).ToList();

            return View(model);
        }

        [HttpGet]
        [Route("edit-publication/{id}")]
        public async Task<IActionResult> EditPublication(Guid id)
        {
            var publications = await _publicationRepository.GetAllPublicationsAsync();
            var publication = publications.FirstOrDefault(p => p.Id == id);

            if (publication == null)
            {
                return NotFound();
            }

            var categories = await _categoryRepository.GetAllCategoriesAsync();

            var model = new PublicationViewModel
            {
                Id = publication.Id,
                Title = publication.Title,
                Description = publication.Description,
                Content = publication.Content ?? string.Empty,
                SeoDescription = publication.SeoDescription,
                Keywords = publication.Keywords,
                ImagePath = publication.ImagePath,
                SelectListCategories = categories.Select(c => new SelectListItem
                {
                    Value = c.Id.ToString(),
                    Text = c.Name,
                    Selected = false
                }).ToList()
            };

            return View(model);
        }

        [HttpPost]
        [Route("edit-publication/{id}")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> EditPublication(Guid id, PublicationViewModel model, IFormFile? image)
        {
            if (id != model.Id)
            {
                return BadRequest();
            }

            if (ModelState.IsValid)
            {
                var publications = await _publicationRepository.GetAllPublicationsAsync();
                var publication = publications.FirstOrDefault(p => p.Id == id);

                if (publication == null)
                {
                    return NotFound();
                }

                if (image != null && image.Length > 0)
                {
                    string uploadsFolder = Path.Combine(_appEnvironment.WebRootPath, "publicationImages");

                    if (!Directory.Exists(uploadsFolder))
                    {
                        Directory.CreateDirectory(uploadsFolder);
                    }

                    string uniqueFileName = Guid.NewGuid().ToString() + "_" + Path.GetFileName(image.FileName);
                    string filePath = Path.Combine(uploadsFolder, uniqueFileName);

                    using (var fileStream = new FileStream(filePath, FileMode.Create))
                    {
                        await image.CopyToAsync(fileStream);
                    }

                    publication.ImagePath = "/publicationImages/" + uniqueFileName;
                    publication.Image = publication.ImagePath;
                    publication.FullImageName = publication.ImagePath;
                }

                publication.Title = model.Title;
                publication.Description = model.Description;
                publication.SeoDescription = model.SeoDescription;
                publication.Keywords = model.Keywords;
                publication.Content = model.Content;

                await _publicationRepository.UpdatePublicationAsync(publication);

                return RedirectToAction("Index");
            }

            var categories = await _categoryRepository.GetAllCategoriesAsync();
            model.SelectListCategories = categories.Select(c => new SelectListItem
            {
                Value = c.Id.ToString(),
                Text = c.Name
            }).ToList();

            return View(model);
        }

        [HttpGet]
        [Route("create-category")]
        public IActionResult CreateCategory()
        {
            return View();
        }

        [HttpPost]
        [Route("create-category")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> CreateCategory(CategoryViewModel model)
        {
            if (ModelState.IsValid)
            {
                var category = new Category
                {
                    Id = Guid.NewGuid(),
                    Name = model.Name
                };

                await _categoryRepository.AddCategoryAsync(category);
                return RedirectToAction("Index");
            }

            return View(model);
        }

        [HttpGet]
        [Route("edit-category/{id}")]
        public async Task<IActionResult> EditCategory(Guid id)
        {
            var categories = await _categoryRepository.GetAllCategoriesAsync();
            var category = categories.FirstOrDefault(c => c.Id == id);

            if (category == null)
            {
                return NotFound();
            }

            var model = new CategoryViewModel
            {
                Id = category.Id,
                Name = category.Name
            };

            return View(model);
        }

        [HttpPost]
        [Route("edit-category/{id}")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> EditCategory(CategoryViewModel model)
        {
            if (ModelState.IsValid)
            {
                var categories = await _categoryRepository.GetAllCategoriesAsync();
                var category = categories.FirstOrDefault(c => c.Id == model.Id);

                if (category != null)
                {
                    category.Name = model.Name;
                    await _categoryRepository.UpdateCategoryAsync(category);
                }

                return RedirectToAction("Index");
            }

            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteCategory(Guid id)
        {
            var categories = await _categoryRepository.GetAllCategoriesAsync();
            var category = categories.FirstOrDefault(c => c.Id == id);

            if (category != null)
            {
                await _categoryRepository.DeleteCategoryAsync(category);
            }

            return RedirectToAction("Index");
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeletePublication(Guid id)
        {
            var publications = await _publicationRepository.GetAllPublicationsAsync();
            var publication = publications.FirstOrDefault(p => p.Id == id);

            if (publication != null)
            {
                await _publicationRepository.DeletePublicationAsync(publication);
            }

            return RedirectToAction("Index");
        }
    }
}