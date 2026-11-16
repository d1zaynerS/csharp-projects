using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Http;

namespace MultyFormMessage.Models
{
    public class RegisterViewModel
    {
        [Required(ErrorMessage = "Введіть Email")]
        [EmailAddress(ErrorMessage = "Не коректний E-Mail")]
        public string Email { get; set; } = string.Empty;

        [Required(ErrorMessage = "Введіть пароль")]
        [DataType(DataType.Password)]
        public string Password { get; set; } = string.Empty;

        [Required(ErrorMessage = "Підтвердіть пароль")]
        [Compare("Password", ErrorMessage = "Паролі не збігаються")]
        [DataType(DataType.Password)]
        public string PasswordConfirm { get; set; } = string.Empty;
    }

    public class LoginViewModel
    {
        [Required(ErrorMessage = "Введіть Email")]
        public string Email { get; set; } = string.Empty;

        [Required(ErrorMessage = "Введіть пароль")]
        [DataType(DataType.Password)]
        public string Password { get; set; } = string.Empty;

        public bool RememberMe { get; set; }
        public string? ReturnUrl { get; set; }
    }

    public class EmailSendViewModel
    {
        [Required(ErrorMessage = "Вкажіть адресата")]
        public string ToAddresses { get; set; } = string.Empty;

        [Required(ErrorMessage = "Вкажіть тему листа")]
        public string Subject { get; set; } = string.Empty;

        [Required(ErrorMessage = "Введіть текст листа")]
        public string Content { get; set; } = string.Empty;

        public IFormFileCollection? Attachments { get; set; }
    }
}