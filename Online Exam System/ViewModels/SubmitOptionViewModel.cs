using System.ComponentModel.DataAnnotations;

namespace Online_Exam_System.ViewModels
{
	public class SubmitOptionViewModel
	{
		[Required(ErrorMessage = "you should answer all questions")]
		public string OptionId { get; set; }
	}
}
