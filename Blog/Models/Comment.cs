using System;
using System.ComponentModel.DataAnnotations;

namespace Blog.Models
{
    public class Comment
    {
        public int Id { get; set; }
        public Guid PublicationId { get; set; }

        [Required(ErrorMessage = "Введіть ім'я")]
        public string AuthorName { get; set; } = null!;

        public string? Email { get; set; }

        [Required(ErrorMessage = "Введіть текст коментаря")]
        public string Message { get; set; } = null!;

        public DateTime CreatedAt { get; set; } = DateTime.Now;
    }
}