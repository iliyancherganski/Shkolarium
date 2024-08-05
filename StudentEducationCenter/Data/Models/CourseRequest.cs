using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using StudentEducationCenter.Data.Enums;

namespace StudentEducationCenter.Data.Models
{
    public class CourseRequest
    {
        [Key]
        public int Id { get; set; }

        [ForeignKey(nameof(Child))]
        public int ChildId { get; set; }
        public Child Child { get; set; } = null!;

        [ForeignKey(nameof(Course))]
        public int CourseId { get; set; }
        public Course Course { get; set; } = null!;

        public RequestStatus Status { get; set; }
    }
}
