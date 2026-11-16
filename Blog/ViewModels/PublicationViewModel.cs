using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace Blog.ViewModels
{
    public class PublicationViewModel
    {
        public Guid Id { get; set; }

        [Required(ErrorMessage = "Вкажіть заголовок")]
        public string Title { get; set; } = null!;

        public string? Description { get; set; }
        public string? SeoDescription { get; set; }
        public string? Keywords { get; set; }

        [Required(ErrorMessage = "Вкажіть текст публікації")]
        [Display(Name = "Вміст")]
        public string Content { get; set; } = null!;

        public string? ImagePath { get; set; }
        public string? Image { get; set; }
        public string? ImageFullName { get; set; }

        public IFormFile? File { get; set; }

        public List<string>? SelectedCategories { get; set; }
        public List<SelectListItem>? SelectListCategories { get; set; }
    }
}