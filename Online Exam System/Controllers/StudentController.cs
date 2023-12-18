using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Online_Exam_System.Data;
using Online_Exam_System.Models;
using Online_Exam_System.ViewModels;

namespace Online_Exam_System.Controllers
{
	[Authorize(Roles = "Student")]
	public class StudentController : Controller
	{
		private readonly UserManager<User> _userManager;
		private readonly RoleManager<IdentityRole> _roleManager;
		private readonly SignInManager<User> _signInManager;
		private readonly ApplicationDbContext _context;

		public StudentController(UserManager<User> userManager, SignInManager<User> signInManager, ApplicationDbContext context, RoleManager<IdentityRole> roleManager)
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
		public IActionResult TakeExam()
		{
			return View();
		}

		public bool IsNotCompleted(Exam exam)
		{
			var examQuestions = _context.Questions.Where(q => q.ExamID == exam.ExamID).ToList();
			bool isNotCompleted = examQuestions.Count == 0 ? true : examQuestions.Any(question => question.OptionsNumber > _context.Options.Count(o => o.QuestionID == question.QuestionID));
			return isNotCompleted;
		}

		public bool CheckExamDate(Exam exam)
		{
			return exam.StartTime > DateTime.Now || exam.EndTime < DateTime.Now ? true : false;
		}

		[HttpPost]
		public IActionResult TakeExam(TakeExamViewModel takeExamViewModel)
		{
			if (!ModelState.IsValid) return View(takeExamViewModel);
			var exam = _context.Exams.FirstOrDefault(e => e.ExamCode == takeExamViewModel.ExamCode);
			// exam does not exist with entered exam code
			if (exam == null)
			{
				TempData["Error"] = "There is no exam have this code";
				return View(takeExamViewModel);
			}
			else // the exam is exist
			{
				if (IsNotCompleted(exam))
				{
					TempData["Error"] = "This exam is not completed yet";
					return View(takeExamViewModel);
				}
				else if (CheckExamDate(exam))
				{
					TempData["Error"] = "This exam does not started yet or you exceeded the access time";
					return View(takeExamViewModel);
				}
				else // everything is good
				{
					return RedirectToAction("Exam", "Student", new { id = takeExamViewModel.ExamCode });
				}
			}
		}

		public async Task<IActionResult> Exam(string id)
		{
			if (id == null) return View("Error");
			var exam = _context.Exams.FirstOrDefault(e => e.ExamCode == id);
			// Getting User id
			var currentUserId = await _userManager.GetUserAsync(User);
			var userId = currentUserId?.Id;
			if (exam == null)
			{
				TempData["Error"] = "There is no exam have this code";
				return RedirectToAction("TakeExam");
			}
			if (IsNotCompleted(exam))
			{
				return RedirectToAction("TakeExam");
			}
			else if (CheckExamDate(exam))
			{
				return RedirectToAction("TakeExam");
			}
			else if(_context.ExamAttempts.Any(examAttmpt => examAttmpt.ExamID == exam.ExamID && examAttmpt.UserID == userId))
			{
				TempData["Error"] = "You take this exam before";
				return RedirectToAction("TakeExam");
			}
			var examQuestions = _context.Questions.Where(q => q.ExamID == exam.ExamID).ToList();
			var options = _context.Questions
						.Where(q => q.ExamID == exam.ExamID)
						.SelectMany(question => _context.Options.Where(option => option.QuestionID == question.QuestionID))
						.ToList();
			var instructorName = _context.Users.FirstOrDefault(user => user.Id == exam.InstructorID)?.name;

            var viewModel = new ExamViewModel {
				Exam = exam,
				Questions = examQuestions,
				Options = options,
				InstructorName = instructorName,
				StartTime = DateTime.Now,
			};
			return View(viewModel);
		}

		[HttpPost]
		public async Task<IActionResult> Exam(string id, ExamViewModel examViewModel)
		{
			var exam = _context.Exams.FirstOrDefault(e => e.ExamCode == id);
			if (exam == null)
			{
				TempData["Error"] = "There is no exam have this code";
				return RedirectToAction("TakeExam");
			}
			var examQuestions = _context.Questions.Where(q => q.ExamID == exam.ExamID).ToList();
			var options = _context.Questions
						.Where(q => q.ExamID == exam.ExamID)
						.SelectMany(question => _context.Options.Where(option => option.QuestionID == question.QuestionID))
						.ToList();

			examViewModel.Exam = exam;
			examViewModel.Questions = examQuestions;
			examViewModel.Options = options;

			if (id == null) return View("Error");
			if (!ModelState.IsValid) return View(examViewModel);
			// Getting User id
			var currentUserId = await _userManager.GetUserAsync(User);
			var userId = currentUserId?.Id;
			//make user attmpt
			var userAttmpt = new ExamAttempt
			{
				ExamID = exam.ExamID,
				UserID = userId,
				StartTime = examViewModel.StartTime,
				EndTime = DateTime.Now,
			};

			TimeSpan userAttmptDuration = (TimeSpan)(userAttmpt.EndTime - userAttmpt.StartTime);

			if ((int)userAttmptDuration.TotalMinutes > exam.DurationMinutes)
			{
				ModelState.AddModelError("", "Exam duration exceeded");
				return View("Exam", examViewModel);
			}

			_context.ExamAttempts.Add(userAttmpt);
			_context.SaveChanges();
			for (int i = 0; i < examViewModel.Questions.Count; i++)
			{
				var userAnswer = new UserAnswer
				{
					AttemptID = userAttmpt.AttemptID,
					QuestionID = examViewModel.Questions[i].QuestionID,
					OptionID = int.Parse(examViewModel.OptionId[i].OptionId),
				};

				_context.Add(userAnswer);
			}

			_context.SaveChanges();


			var userCorrectAnswer = _context.UserAnswers
				.Where(answer => answer.AttemptID == userAttmpt.AttemptID)
				.Join(
					_context.Options.Where(o => o.IsCorrect),
					userAnswer => userAnswer.OptionID,
					correctOption => correctOption.OptionID,
					(userAnswer, correctOption) => userAnswer
				);

			int score = 0;

			foreach (var answer in userCorrectAnswer)
			{
				int questionPoints = answer.Question.QuestionPoints;
				score += questionPoints;
			}

			var examScore = new ExamResult
			{
				AttemptID = userAttmpt.AttemptID,
				Score = score,
			};
			_context.ExamResults.Add(examScore);
			_context.SaveChanges();
			return RedirectToAction("Index");
		}
        public IActionResult ExamsHistory(string id)
        {
            var examsAndResults = _context.ExamAttempts
            .Where(ea => ea.UserID == id)
            .Include(ea => ea.Exam)
            .Select(ea => new { Exam = ea.Exam, Result = _context.ExamResults.FirstOrDefault(er => er.AttemptID == ea.AttemptID) })
            .ToList();

            var viewModel = examsAndResults
            .Select(ea => (exam: ea.Exam, result: ea.Result))
            .ToList();

            return View(viewModel);
        }
        public IActionResult Settings(string id)
		{
			var user = _context.Users.FirstOrDefault(user => user.Id == id);
			if (user == null) return View("Error");
			var settingsVM = new SettingsViewModel
			{
				EmailAddress = user.Email,
				Name = user.name,
				Password = "",
				ConfirmPassword = "",
			};
			return View(settingsVM);
		}

		[HttpPost]
		public async Task<IActionResult> Settings(string id, SettingsViewModel settingsViewModel)
		{
			if (!ModelState.IsValid)
			{
				ModelState.AddModelError("", "Failed to edit");
				return View("Settings", settingsViewModel);
			}
			var user = _context.Users.FirstOrDefault(x => x.Id == id);
			if (user != null)
			{
				user.name = settingsViewModel.Name;
				user.Email = settingsViewModel.EmailAddress;
				if (settingsViewModel.Password != null)
				{
					await _userManager.RemovePasswordAsync(user);
					await _userManager.AddPasswordAsync(user, settingsViewModel.Password);
				}
				if (settingsViewModel.ProfilePicture != null)
				{
					// Process profile picture
					if (settingsViewModel.ProfilePicture != null && settingsViewModel.ProfilePicture.Length > 0)
					{
						using (var memoryStream = new MemoryStream())
						{
							await settingsViewModel.ProfilePicture.CopyToAsync(memoryStream);
							user.ProfilePictureData = memoryStream.ToArray();
						}
					}
				}
				var editedUserResponse = await _userManager.UpdateAsync(user);
				return RedirectToAction("Index", "Student");
			}
			else
			{
				return View(settingsViewModel);
			}
		}

	}
}