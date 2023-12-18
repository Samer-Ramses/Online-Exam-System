using Online_Exam_System.Models;

namespace Online_Exam_System.ViewModels
{
    public class StatsViewModel
    {
        public User User { get; set; }
        public Exam Exam { get; set; }
        public ExamResult ExamResult { get; set; }
    }
}
