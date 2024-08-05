namespace StudentEducationCenter.ViewModels.User
{
	public class RegisterStaffViewModel : RegisterViewModel
	{
        public RegisterStaffViewModel()
        {
            SpecialtyIds = new List<int>();
        }

        public string Roles { get; set; } = null!;

		public List<int> SpecialtyIds { get; set; } = null!;

	}
}
