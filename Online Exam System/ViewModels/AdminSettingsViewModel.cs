using System.ComponentModel.DataAnnotations;

namespace Online_Exam_System.ViewModels
{
	public class AdminSettingsViewModel
	{
		[Display(Name = "Email address")]
		[Required(ErrorMessage = "Email address is required")]
		public string EmailAddress { get; set; }
		[Display(Name = "Name")]
		[Required(ErrorMessage = "Name is required")]
		public string Name { get; set; }
		[Required]
		[DataType(DataType.Password)]
		public string Password { get; set; }
		[Display(Name = "Confirm Password")]
		[Required(ErrorMessage = "Confirm password is required")]
		[DataType(DataType.Password)]
		[Compare("Password", ErrorMessage = "Password do not match")]
		public string ConfirmPassword { get; set; }
	}
}
