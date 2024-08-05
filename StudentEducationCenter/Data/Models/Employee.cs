using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace StudentEducationCenter.Data.Models
{
    public class Employee
    {
        public Employee()
        {
            Courses = new List<Course>();
        }
        public Employee(User user)
		{
			User = user;
            UserId = user.Id;
		}

		[Key]
        public int Id { get; set; }

        [ForeignKey(nameof(User))]
        public string UserId { get; set; } = null!;
        public User User { get; set; } = null!;

        public List<Course> Courses { get; set; } = new List<Course>();
    }
}
