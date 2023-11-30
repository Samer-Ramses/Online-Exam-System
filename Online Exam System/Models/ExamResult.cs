using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace Online_Exam_System.Models
{
    public class ExamResult
    {
        [Key]
        public int AttemptID { get; set; }

        [Required]
        public int Score { get; set; }

        [ForeignKey("AttemptID")]
        public virtual ExamAttempt ExamAttempt { get; set; }
    }
}
