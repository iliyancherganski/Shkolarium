namespace StudentEducationCenter.ViewModels.Employee
{
    public class CourseEmployeeViewModel
    {
        public int CourseId { get; set; }

        public string CourseTypeName { get; set; } = null!;
        public string CourseStartDateToString { get; set; } = null!;
        public string CourseEndDateToString { get; set; } = null!;
        public string CourseAgeGroupName { get; set; } = null!;
        public string? CourseTeachers { get; set; }
        public int EmployeeId { get; set; }

    }
}
