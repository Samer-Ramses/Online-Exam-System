using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace Online_Exam_System.Models
{
    public class Question
    {
        [Key]
        public int QuestionID { get; set; }

        [Required]
        public int ExamID { get; set; }

        [Required]
        public string QuestionText { get; set; }

        [ForeignKey("ExamID")]
        public virtual Exam Exam { get; set; }
    }
}
