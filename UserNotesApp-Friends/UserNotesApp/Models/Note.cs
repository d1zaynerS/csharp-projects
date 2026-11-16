using System;
using System.ComponentModel.DataAnnotations;

namespace UserNotesApp.Models
{
    public class Note
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "Введіть заголовок")]
        [Display(Name = "Заголовок")]
        public string Title { get; set; } = string.Empty;

        [Display(Name = "Текст нотатки")]
        public string Content { get; set; } = string.Empty;

        public DateTime CreatedAt { get; set; } = DateTime.Now;

        public string UserId { get; set; } = string.Empty;
        public User? User { get; set; }
    }
}