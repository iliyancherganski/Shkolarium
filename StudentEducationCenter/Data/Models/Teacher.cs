using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace StudentEducationCenter.Data.Models
{
	public class Teacher
	{
		public Teacher()
		{
			TeacherCourses = new List<TeacherCourse>();
			TeacherSpecialties = new List<TeacherSpecialty>();
		}
		public Teacher(User user)
		{
			TeacherCourses = new List<TeacherCourse>();
			TeacherSpecialties = new List<TeacherSpecialty>();
			User = user;
			UserId = user.Id;
		}

		[Key]
		public int Id { get; set; }

		[ForeignKey(nameof(User))]
		public string UserId { get; set; } = null!;
		public User User { get; set; } = null!;

		public List<TeacherCourse> TeacherCourses { get; set; } = new List<TeacherCourse>();
		public List<TeacherSpecialty> TeacherSpecialties { get; set; } = new List<TeacherSpecialty>();
	}
}
