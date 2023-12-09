using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Online_Exam_System.ViewModels
{
    public class CreateExamViewModel : IValidatableObject
    {
        [Display(Name = "Exam Name")]
        [Required(ErrorMessage = "Exam name is required")]
        public string ExamName { get; set; }

        [Display(Name = "Exam Code")]
        [Required(ErrorMessage = "Exam code is required")]
        public string ExamCode { get; set; }

        [Display(Name = "Start date")]
        [Required(ErrorMessage = "Start date is required")]
        public DateTime StartDate { get; set; }

        [Display(Name = "Start time")]
        [Required(ErrorMessage = "Start time is required")]
        public TimeSpan StartTime { get; set; }

        [Display(Name = "End access date")]
        [Required(ErrorMessage = "End date is required")]
        public DateTime EndDate { get; set; }

        [Display(Name = "End access time")]
        [Required(ErrorMessage = "End access time is required")]
        public TimeSpan EndTime { get; set; }

        [Display(Name = "Duration in minutes")]
        [Required(ErrorMessage = "Duration in minutes is required")]
        public int DurationMinutes { get; set; }

        [Display(Name = "Exam's points")]
        [Required(ErrorMessage = "Exam's points is required")]
        public int ExamPoints { get; set; }

        [Display(Name = "Success degree")]
        [Required(ErrorMessage = "Success degree is required")]
        public int SuccessDegree { get; set; }

        public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            DateTime currentDateTime = DateTime.Now;
            var start = StartDate.Add(StartTime);

            if (start <= currentDateTime)
            {
                yield return new ValidationResult("Start date and time must be greater than the current date and time.", new[] { nameof(StartTime), nameof(StartDate) });
            }

            var end = EndDate.Add(EndTime);

            if (end < start)
            {
                yield return new ValidationResult("End date and time must be greater than or equal start date and time.", new[] { nameof(EndTime), nameof(EndDate) });
            }
        }
    }
}