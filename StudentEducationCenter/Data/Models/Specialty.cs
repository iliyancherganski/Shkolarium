using System.ComponentModel.DataAnnotations;

namespace StudentEducationCenter.Data.Models
{
    public class Specialty
    {
        [Key]
        public int Id { get; set; }

        [Required]
        [StringLength(50)]
        public string Name { get; set; } = null!;

        public List<TeacherSpecialty> TeachersSpecialty { get; set; } = new List<TeacherSpecialty>();
    }
}
