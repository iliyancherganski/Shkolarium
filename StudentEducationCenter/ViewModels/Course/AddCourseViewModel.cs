using StudentEducationCenter.Data.Models;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace StudentEducationCenter.ViewModels.Course
{
    public class AddCourseViewModel
    {
        public int Id { get; set; }
        [Required]
        public int EmployeeId { get; set; } = 0;

        [Required]
        public int AgeGroupId { get; set; }

        [Required]
        public DateTime StartDate { get; set; }

        [Required]
        public DateTime EndDate { get; set; }

        [Required]
        [Range(0,  100000, ErrorMessage = "Цената трябва да бъде число между 0 и 100000.")]
        public decimal Price { get; set; }

        [Required]
        public int CourseTypeId { get; set; }
        

        [Required]
        public List<int> TeacherIds { get; set; } = new List<int>();
    }
}
