using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace StudentEducationCenter.Data.Models
{
    public class Child
    {
        [Key]
        public int Id { get; set; }

        [Required]
        [MaxLength(50)]
        public string FirstName { get; set; } = null!;

        [Required]
        [MaxLength(50)]
        public string LastName { get; set; } = null!;

        [ForeignKey(nameof(Parent))]
        public int ParentId { get; set; }
        public Parent Parent { get; set; } = null!;

        [MaxLength(15)]
        public string? PhoneNumber { get; set; }

        public List<CourseRequest> CourseRequests { get; set; } = new List<CourseRequest>();
    }
}
