using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace Online_Exam_System.Models
{
    public class UserAnswer
    {
        [Key]
        public int AttemptID { get; set; }

        public int QuestionID { get; set; }

        [Required]
        public int OptionID { get; set; }

        [ForeignKey("AttemptID")]
        public virtual ExamAttempt ExamAttempt { get; set; }

        [ForeignKey("QuestionID")]
        public virtual Question Question { get; set; }

        [ForeignKey("OptionID")]
        public virtual Option Option { get; set; }
    }
}
