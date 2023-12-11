using Online_Exam_System.Models;

namespace Online_Exam_System.ViewModels
{
    public class CurrentExamsViewModel
    {
        public IEnumerable<Exam> Exams { get; set; }
        public Dictionary<int, int> ExamQuestionCounts { get; set; }
        public Dictionary<int, int> ExamQuestionsPointSum { get; set; }
        public bool isNotCompleted { get; set; }
    }
}