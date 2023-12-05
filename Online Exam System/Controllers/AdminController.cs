using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Online_Exam_System.Controllers
{
	[Authorize(Roles = "Admin")]
	public class AdminController : Controller
	{
		public IActionResult Index()
		{
			return View();
		}

		public IActionResult CreateUser()
		{
			return View();
		}

		public IActionResult AllUsers()
		{
			return View();
		}

		public IActionResult Settings()
		{
			return View();
		}
	}
}
