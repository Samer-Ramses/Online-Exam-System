using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Online_Exam_System.Controllers
{
    [Authorize(Roles = "Student")]
    public class StudentController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
        public IActionResult TakeExam() 
        {
            return View();
        }
        public IActionResult ExamsHistory() 
        {
            return View();  
        }
        public IActionResult Settings() 
        {
            return View();
        }
    }
}
