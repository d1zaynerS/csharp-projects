using System.ComponentModel.DataAnnotations;

namespace Blog.ViewModels
{
    public class CategoryViewModel
    {
        [Key]
        public Guid? Id { get; set; }
        [Display(Name = "Назва")]
        [Required(ErrorMessage = "Не вказано назву категорії")]
        public string? Name { get; set; }
        [Display(Name = "Опис")]
        public string? Description { get; set; }
    }
}
