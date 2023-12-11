using System.ComponentModel.DataAnnotations;

namespace Online_Exam_System.ViewModels
{
    public class AddQuestionViewModel
    {
        [Display(Name = "Question Title")]
        [Required(ErrorMessage = "Question title")]
        public string QuestionText { get; set; }
        [Display(Name = "Question Points")]
        [Required(ErrorMessage = "Question points")]
        [Range(1, 5, ErrorMessage = "The question points should be between 1 and 5")]
        public int QuestionPoints { get; set; }
        [Display(Name = "Options Numbers")]
        [Required(ErrorMessage = "Question Numbers")]
        [Range(2, 10, ErrorMessage = "The options number should be between 2 and 10")]
        public int OptionsNumber { get; set; }
    }
}
