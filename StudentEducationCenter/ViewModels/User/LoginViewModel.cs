using Newtonsoft.Json.Serialization;
using System.ComponentModel.DataAnnotations;

namespace StudentEducationCenter.ViewModels.User
{
    public class LoginViewModel
    {
        [Required(ErrorMessage = "Полето 'Имейл' трябва да бъде попълнено.")]
        [EmailAddress]
        public string Email { get; set; } = null!;

        [Required(ErrorMessage = "Полето 'Парола' трябва да бъде попълнено.")]
        [DataType(DataType.Password)]
        public string Password { get; set; } = null!;

        [Display(Name = "Запомни ме?")]
        public bool RememberMe { get; set; }
    }
}
