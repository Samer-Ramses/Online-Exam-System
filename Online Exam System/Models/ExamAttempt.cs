using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace Online_Exam_System.Models
{
    public class ExamAttempt
    {
        [Key]
        public int AttemptID { get; set; }

        [Required]
        public string? UserID { get; set; }

        [Required]
        public int ExamID { get; set; }

        [Required]
        public DateTime StartTime { get; set; }

        public DateTime? EndTime { get; set; }

        [ForeignKey("UserID")]
        public virtual User User { get; set; }

        [ForeignKey("ExamID")]
        public virtual Exam Exam { get; set; }
    }
}
