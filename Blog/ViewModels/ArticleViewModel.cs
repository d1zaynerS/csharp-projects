using System.ComponentModel.DataAnnotations;

namespace Blog.ViewModels
{
    public class ArticleViewModel
    {
        [Required(ErrorMessage = "Вкажіть заголовок")]
        public string Title { get; set; } = null!;

        [Required(ErrorMessage = "Вкажіть текст публікації")]
        public string Content { get; set; } = null!; 

        public string? SeoDescription { get; set; }
        public string? Keywords { get; set; }
    }
}