using System.ComponentModel.DataAnnotations;

namespace StudentEducationCenter.ViewModels.User
{
    public class EditUserViewModel
    {
        public EditUserViewModel()
        {
            SpecialtyIds = new List<int>();
        }

        public string Role { get; set; } = null!;
        public int Id { get; set; }

        [Required(ErrorMessage = "Полето 'Собствено име' трябва да бъде попълнено.")]
        [StringLength(50, ErrorMessage = "{0} трябва да бъде между {2} и {1} символа дълго.", MinimumLength = 2)]
        [Display(Name = "Собствено име")]
        public string FirstName { get; set; } = null!;

        [Required(ErrorMessage = "Полето 'Бащино име' трябва да бъде попълнено.")]
        [StringLength(50, ErrorMessage = "{0} трябва да бъде между {2} и {1} символа дълго.", MinimumLength = 2)]
        [Display(Name = "Бащино име")]
        public string MiddleName { get; set; } = null!;

        [Required(ErrorMessage = "Полето 'Фамилия' трябва да бъде попълнено.")]
        [StringLength(50, ErrorMessage = "{0} трябва да бъде между {2} и {1} символа дълга.", MinimumLength = 2)]
        [Display(Name = "Фамилия")]
        public string LastName { get; set; } = null!;

        [Required]
        [Display(Name = "Град")]
        public int CityId { get; set; }

        [Required(ErrorMessage = "Полето 'Адрес' трябва да бъде попълнено.")]
        [StringLength(150, ErrorMessage = "{0} трябва да бъде между {2} и {1} символа дълъг.", MinimumLength = 2)]
        [Display(Name = "Адрес")]
        public string Address { get; set; } = null!;

        [Required(ErrorMessage = "Полето 'Телефон' трябва да бъде попълнено.")]
		[StringLength(15, ErrorMessage = "{0} трябва да бъде между {2} и {1} символа дълъг.", MinimumLength = 7)]
		[Phone]
        [Display(Name = "Телефон")]
        public string PhoneNumber { get; set; } = null!;

        public List<int> SpecialtyIds { get; set; } = null!;
    }
}
