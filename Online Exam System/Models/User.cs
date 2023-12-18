using Microsoft.AspNetCore.Identity;
using Online_Exam_System.Data;
using System.ComponentModel.DataAnnotations;

namespace Online_Exam_System.Models
{
    public class User : IdentityUser
    {
        public string? Name { get; set; }
        public string? Role { get; set; }
        public byte[]? ProfilePictureData { get; set; }

    }
}
