using System.ComponentModel.DataAnnotations;

namespace Online_Exam_System.ViewModels
{
    public class AddOptionsViewModel
    {
        public AddOptionsViewModel() { 
            Options = new List<OptionViewModel>();
        }
        [Required]
        public List<OptionViewModel> Options { get; set; }
        [Display(Name = "The number of correct answer")]
        [Required(ErrorMessage = "You must enter the number of correct answer")]
        public int CorrectAnswer { get; set; }
    }
}
