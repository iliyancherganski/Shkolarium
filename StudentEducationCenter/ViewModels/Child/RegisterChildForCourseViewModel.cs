using System.ComponentModel.DataAnnotations;

namespace StudentEducationCenter.ViewModels.Child
{
    public class RegisterChildForCourseViewModel
    {
        public int ChildId { get; set; }

        public string? ChildName { get; set; }

        [Required]
        public int CourseId { get; set; } = 0;

        public List<Data.Models.Course> Courses { get; set; } = new List<Data.Models.Course>();
    }
}
