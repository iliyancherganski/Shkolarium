using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace StudentEducationCenter.Data.Models
{
    public class Course
    {
        [Key]
        public int Id { get; set; }

        [ForeignKey(nameof(AgeGroup))]
        public int AgeGroupId { get; set; }
        public AgeGroup AgeGroup { get; set; } = null!;

        [Required]
        public DateTime StartDate { get; set; }

        [Required]
        public DateTime EndDate { get; set; }

        [Required]
        public decimal Price { get; set; }

        [ForeignKey(nameof(Employee))]
        public int EmployeeId { get; set; }
        public Employee Employee { get; set; } = null!;

        [ForeignKey(nameof(CourseType))]
        public int CourseTypeId { get; set; }
        public CourseType CourseType { get; set; } = null!;

        public List<TeacherCourse> TeachersCourse { get; set; } = new List<TeacherCourse>();
        public List<CourseRequest> CourseRequests { get; set; } = new List<CourseRequest>();
    }
}
