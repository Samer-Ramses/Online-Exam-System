using Online_Exam_System.Models;
using System.ComponentModel.DataAnnotations;

namespace Online_Exam_System.ViewModels
{
    public class ExamViewModel
    {
		public string? InstructorName { get; set; }
        public DateTime StartTime { get; set; }
        public Exam? Exam { get; set; }
        public List<Question>? Questions { get; set; }
        public List<Option>? Options { get; set; }
        [Required]
        public List<SubmitOptionViewModel> OptionId { get; set; }

    }
}
