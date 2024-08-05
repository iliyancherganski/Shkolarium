using System.ComponentModel.DataAnnotations;

namespace StudentEducationCenter.ViewModels.Additional
{
    public class CourseTypeViewModel
    {
        [Required(ErrorMessage = "Полето 'Тип курс' трябва да бъде попълнено.")]

        [MinLength(1)]
        [MaxLength(100)]
        public string CourseTypeName { get; set; } = null!;
    }
}
