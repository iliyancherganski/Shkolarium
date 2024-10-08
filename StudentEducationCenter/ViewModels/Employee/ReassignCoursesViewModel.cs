namespace StudentEducationCenter.ViewModels.Employee
{
    public class ReassignCoursesViewModel
    {
        public int EmployeeId { get; set; }
        public string EmployeeFirstName { get; set; } = null!;
        public string EmployeeLastName { get; set; } = null!;

        public List<CourseEmployeeViewModel> coursesEmployees { get; set; }

        public ReassignCoursesViewModel()
        {
            coursesEmployees = new List<CourseEmployeeViewModel>();
        }
    }
}
