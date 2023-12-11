using Online_Exam_System.Models;

namespace Online_Exam_System.ViewModels
{
    public class CurrentQuestionsViewModel
    {
        public IEnumerable<Question> Questions { get; set; }
        public Dictionary<int, int> QuestionOptionsCounts { get; set; }
    }
}
