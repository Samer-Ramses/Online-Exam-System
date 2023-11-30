
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Online_Exam_System.Models
{
    public class Exam
    {
        [Key]
        public int ExamID { get; set; }


        [Required]
        [StringLength(100)]
        public string ExamName { get; set; }

        [Required]
        public DateTime StartTime { get; set; }

        [Required]
        public DateTime EndTime { get; set; }

        [Required]
        public int DurationMinutes { get; set; }

        [Required]
        public string? InstructorID { get; set; }

        [ForeignKey("InstructorID")]
        public virtual User Instructor { get; set; }
    }
}
