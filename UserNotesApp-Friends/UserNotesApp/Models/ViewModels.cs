using System.ComponentModel.DataAnnotations;

namespace UserNotesApp.Models
{
    public class RegisterViewModel
    {
        [Required(ErrorMessage = "Введіть Email")]
        [EmailAddress(ErrorMessage = "Не коректний E-Mail")]
        [Display(Name = "Email")]
        public string Email { get; set; } = string.Empty;

        [Required(ErrorMessage = "Введіть рік народження")]
        [Display(Name = "Рік народження")]
        public int Year { get; set; }

        [Required(ErrorMessage = "Введіть пароль")]
        [DataType(DataType.Password)]
        [Display(Name = "Пароль")]
        public string Password { get; set; } = string.Empty;

        [Required(ErrorMessage = "Підтвердіть пароль")]
        [Compare("Password", ErrorMessage = "Паролі не збігаються")]
        [DataType(DataType.Password)]
        [Display(Name = "Підтвердити пароль")]
        public string PasswordConfirm { get; set; } = string.Empty;
    }

    public class LoginViewModel
    {
        [Required(ErrorMessage = "Введіть Email")]
        [Display(Name = "Email")]
        public string Email { get; set; } = string.Empty;

        [Required(ErrorMessage = "Введіть пароль")]
        [DataType(DataType.Password)]
        [Display(Name = "Пароль")]
        public string Password { get; set; } = string.Empty;

        [Display(Name = "Запам'ятати?")]
        public bool RememberMe { get; set; }

        public string? ReturnUrl { get; set; }
    }

    public class EditUserViewModel
    {
        public string Id { get; set; } = string.Empty;

        [Required(ErrorMessage = "Введіть Email")]
        [Display(Name = "Email")]
        public string Email { get; set; } = string.Empty;

        [Required(ErrorMessage = "Введіть рік народження")]
        [Display(Name = "Рік народження")]
        public int Year { get; set; }
    }

    public class ChangePasswordViewModel
    {
        public string Id { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;

        [Required(ErrorMessage = "Введіть старий пароль")]
        [DataType(DataType.Password)]
        [Display(Name = "Старий пароль")]
        public string OldPassword { get; set; } = string.Empty;

        [Required(ErrorMessage = "Введіть новий пароль")]
        [DataType(DataType.Password)]
        [Display(Name = "Новий пароль")]
        public string NewPassword { get; set; } = string.Empty;
    }
}