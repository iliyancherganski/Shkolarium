using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;
using System.Diagnostics.CodeAnalysis;

namespace StudentEducationCenter.ViewModels.Child
{
    public class AddChildViewModel
    {
        public int? Id { get; set; }

        [Required(ErrorMessage = "Полето 'Собствено име' трябва да бъде попълнено.")]
        [MaxLength(50)]
        [MinLength(1)]
        [Display(Name = "Собствено име")]
        public string FirstName { get; set; } = null!;

        [Required(ErrorMessage = "Полето 'Фамилия' трябва да бъде попълнено.")]
        [MaxLength(50)]
        [MinLength(1)]
        [Display(Name = "Фамилия")]
        public string LastName { get; set; } = null!;

        [Required]
        public int ParentId { get; set; } = 0;

        [AllowNull]
        [Phone]
        [Display(Name = "Телефон на детето")]
        public string? PhoneNumber { get; set; }

        public List<int> CourseIds { get; set; } = new List<int>();
    }
}
