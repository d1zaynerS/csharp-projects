using System;
using System.ComponentModel.DataAnnotations;

namespace UserNotesApp.Models
{
    public class Article
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "Введіть заголовок статті")]
        [Display(Name = "Заголовок")]
        public string Title { get; set; } = string.Empty;

        [Required(ErrorMessage = "Введіть опис статті")]
        [Display(Name = "Опис")]
        public string Content { get; set; } = string.Empty;

        [Display(Name = "Дата публікації")]
        public DateTime CreatedAt { get; set; } = DateTime.Now;

        // Власник (автор) статті
        public string UserId { get; set; } = string.Empty;
        public User? User { get; set; }
    }
}