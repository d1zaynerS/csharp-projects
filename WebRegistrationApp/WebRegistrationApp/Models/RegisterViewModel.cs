using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;

namespace WebRegistrationApp.Models
{
    public class RegisterViewModel
    {
 
        [Required(ErrorMessage = "Ім'я обов'язкове для заповнення")]
        public string FirstName { get; set; } = string.Empty;


        [Required(ErrorMessage = "Прізвище обов'язкове для заповнення")]
        public string LastName { get; set; } = string.Empty;


        [Required(ErrorMessage = "Ім'я користувача обов'язкове")]
        [StringLength(20, ErrorMessage = "Ім'я користувача не може бути довшим за 20 символів")]
        [RegularExpression(@"^[a-zA-Z0-9_]+$", ErrorMessage = "Дозволені лише літери, цифри та підкреслення")]
        [Remote(action: "VerifyUsername", controller: "Account", ErrorMessage = "Це ім'я користувача вже зайняте")]
        public string Username { get; set; } = string.Empty;


        [Required(ErrorMessage = "Email обов'язковий")]
        [EmailAddress(ErrorMessage = "Некоректний формат Email")]
        public string Email { get; set; } = string.Empty;


        [Required(ErrorMessage = "Пароль обов'язковий")]
        [StringLength(100, MinimumLength = 6, ErrorMessage = "Пароль має бути від 6 до 100 символів")]
        [DataType(DataType.Password)]
        public string Password { get; set; } = string.Empty;


        [Required(ErrorMessage = "Підтвердження пароля обов'язкове")]
        [StringLength(100)]
        [DataType(DataType.Password)]
        [Compare("Password", ErrorMessage = "Паролі не збігаються")]
        public string ConfirmPassword { get; set; } = string.Empty;


        [Range(18, 100, ErrorMessage = "Вік повинен бути від 18 до 100 років")]
        public int Age { get; set; }


        [Phone(ErrorMessage = "Некоректний номер телефону")]
        public string? PhoneNumber { get; set; }


        [CreditCard(ErrorMessage = "Некоректний номер кредитної картки")]
        public string? CreditCardNumber { get; set; }


        [Url(ErrorMessage = "Некоректний формат URL-адреси")]
        public string? Website { get; set; }


        [ValidateNever]
        public bool TermsOfService { get; set; }
    }
}