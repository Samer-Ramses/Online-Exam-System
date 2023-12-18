using System.ComponentModel.DataAnnotations;

namespace Online_Exam_System.ViewModels
{
    public class TakeExamViewModel
    {
        [Display(Name = "Exam Code")]
        [Required(ErrorMessage = "Exam Code is required")]
        public string ExamCode { get; set; }
    }
}
