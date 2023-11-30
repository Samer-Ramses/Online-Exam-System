using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace Online_Exam_System.Models
{
    public class Option
    {
        [Key]
        public int OptionID { get; set; }

        [Required]
        public int QuestionID { get; set; }

        [Required]
        public string OptionText { get; set; }

        [Required]
        public bool IsCorrect { get; set; }

        [ForeignKey("QuestionID")]
        public virtual Question Question { get; set; }
    }
}
