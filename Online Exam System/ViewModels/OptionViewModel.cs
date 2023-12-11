using System.ComponentModel.DataAnnotations;

namespace Online_Exam_System.ViewModels
{
    public class OptionViewModel
    {
        [Display(Name = "Option Text")]
        [Required(ErrorMessage = "Option text is required")]
        public string OptionText { get; set; }
    }
}
