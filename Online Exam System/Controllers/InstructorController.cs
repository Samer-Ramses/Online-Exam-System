using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Online_Exam_System.Data;
using Online_Exam_System.Models;
using Online_Exam_System.ViewModels;

namespace Online_Exam_System.Controllers
{
	[Authorize(Roles = "Instructor")]
	public class InstructorController : Controller
	{
        private readonly UserManager<User> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly SignInManager<User> _signInManager;
        private readonly ApplicationDbContext _context;

        public InstructorController(UserManager<User> userManager, SignInManager<User> signInManager, ApplicationDbContext context, RoleManager<IdentityRole> roleManager)
        {
            _userManager = userManager;
            _roleManager = roleManager;
            _signInManager = signInManager;
            _context = context;
        }

        public IActionResult Index()
		{
			return View();
		}

		public IActionResult CreateExam()
		{
			return View();
		}

		[HttpPost]
		public async Task<IActionResult> CreateExam(CreateExamViewModel createExamViewModel)
		{
			if(!ModelState.IsValid) return View(createExamViewModel);
            var currentUser = await _userManager.GetUserAsync(User);
            var exam = new Exam()
			{
				ExamName = createExamViewModel.ExamName,
				ExamCode = createExamViewModel.ExamCode,
				StartTime = createExamViewModel.StartDate.Add(createExamViewModel.StartTime),
				EndTime = createExamViewModel.EndDate.Add(createExamViewModel.EndTime),
				DurationMinutes = createExamViewModel.DurationMinutes,
				ExamPoints = createExamViewModel.ExamPoints,
				SuccessDegree = createExamViewModel.SuccessDegree,
				InstructorID = currentUser.Id.ToString(),
			};

			_context.Add(exam);
			_context.SaveChanges();
			return RedirectToAction("Index");
        }

		public IActionResult CurrentExams() { 
			return View();
		}

		public IActionResult Settings()
		{
			return View();
		}
	}
}
