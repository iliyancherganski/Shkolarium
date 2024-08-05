using System.ComponentModel.DataAnnotations;

namespace StudentEducationCenter.ViewModels.Additional
{
    public class AgeGroupViewModel
    {
        [Required(ErrorMessage = "Полето 'Възрастова група' трябва да бъде попълнено.")]
        [MinLength(1)]
        [MaxLength(100)]
        public string AgeGroupName { get; set; } = null!;
    }
}
