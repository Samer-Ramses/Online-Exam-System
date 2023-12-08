using System.ComponentModel.DataAnnotations;

namespace Online_Exam_System.ViewModels
{
    public class CreateUserViewModel
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
        [Required(ErrorMessage = "Image is reduired")]
        [Display(Name = "Profile Picture")]
        public IFormFile ProfilePicture { get; set; }
        [Display(Name = "Role")]
        [Required(ErrorMessage = "Role is required")]
        public string Role { get; set; }
    }
}