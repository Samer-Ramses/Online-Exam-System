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
            if (!ModelState.IsValid) return View(createExamViewModel);
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

        public async Task<IActionResult> CurrentExams()
        {
            // Getting User id
            var currentUserId = await _userManager.GetUserAsync(User);
            var userId = currentUserId?.Id;
            // Getting all exams
            var exams = _context.Exams.ToList();
            // Fillter the exams by current user "instructor"
            var userExams = exams.Where(exam => exam.InstructorID == userId).ToList();

            var examQuestionCounts = userExams.ToDictionary(exam => exam.ExamID, exam => _context.Questions.Count(q => q.ExamID == exam.ExamID));
            var examQuestionsPointsSum = userExams.ToDictionary(exam => exam.ExamID, exam => _context.Questions.Where(q => q.ExamID == exam.ExamID).Sum(q => q.QuestionPoints));
            bool isNotCompleted = userExams.Any(exam => _context.Questions.Where(q => q.ExamID == exam.ExamID).Any(question => question.OptionsNumber > _context.Options.Count(o => o.QuestionID == question.QuestionID)));
            var viewModel = new CurrentExamsViewModel()
            {
                Exams = userExams,
                ExamQuestionCounts = examQuestionCounts,
                ExamQuestionsPointSum = examQuestionsPointsSum,
                isNotCompleted = isNotCompleted,
            };

            return View(viewModel);
        }

        public IActionResult EditExam(string id)
        {
            var exam = _context.Exams.FirstOrDefault(x => x.ExamID.ToString() == id);
            if (exam == null) return View("Error");
            var editExamVM = new CreateExamViewModel()
            {
                ExamName = exam.ExamName,
                ExamCode = exam.ExamCode,
                StartDate = exam.StartTime.Date,
                StartTime = exam.StartTime.TimeOfDay,
                EndDate = exam.EndTime.Date,
                EndTime = exam.EndTime.TimeOfDay,
                DurationMinutes = exam.DurationMinutes,
                ExamPoints = exam.ExamPoints,
                SuccessDegree = exam.SuccessDegree,
            };
            return View(editExamVM);
        }

        [HttpPost]
        public IActionResult EditExam(string id, CreateExamViewModel editExamViewModel)
        {
            if (!ModelState.IsValid)
            {
                ModelState.AddModelError("", "Failed to edit");
                return View("EditExam", editExamViewModel);
            }
            var exam = _context.Exams.FirstOrDefault(exam => exam.ExamID.ToString() == id);
            if (exam != null)
            {
                exam.ExamName = editExamViewModel.ExamName;
                exam.ExamCode = editExamViewModel.ExamCode;
                exam.StartTime = editExamViewModel.StartDate.Add(editExamViewModel.StartTime);
                exam.EndTime = editExamViewModel.EndDate.Add(editExamViewModel.EndTime);
                exam.DurationMinutes = editExamViewModel.DurationMinutes;
                exam.ExamPoints = editExamViewModel.ExamPoints;
                exam.SuccessDegree = editExamViewModel.SuccessDegree;
                _context.Exams.Update(exam);
                _context.SaveChanges();
                return RedirectToAction("CurrentExams", "Instructor");
            }
            else
            {
                return View(editExamViewModel);
            }

        }

        public IActionResult DeleteExam(string id)
        {
            var exam = _context.Exams.FirstOrDefault(x => x.ExamID.ToString() == id);
            if (exam == null) return View("Error");
            _context.Exams.Remove(exam);
            _context.SaveChanges();
            return RedirectToAction("Index");
        }

        public IActionResult CurrentQuestion(int id)
        {
            var questions = _context.Questions.ToList();
            var examQuestions = questions.Where(exam => exam.ExamID == id).ToList();
            var questionOptionsCounts = examQuestions.ToDictionary(question => question.QuestionID, question => _context.Options.Count(q => q.QuestionID == question.QuestionID));
            var viewModel = new CurrentQuestionsViewModel
            {
                Questions = examQuestions,
                QuestionOptionsCounts = questionOptionsCounts,
            };
            return View(viewModel);
        }

        public IActionResult AddQuestionForm()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> AddQuestionForm(int id, AddQuestionViewModel addQuestionViewModel)
        {
            if(!ModelState.IsValid) return View(addQuestionViewModel);
            if(id == null) return View("Error");
            var currentUserId = await _userManager.GetUserAsync(User);
            var userId = currentUserId?.Id;
            var theExam = _context.Exams.FirstOrDefault(x => x.ExamID == id);
            var pointsSum = _context.Questions.Where(q => q.ExamID == theExam.ExamID).Sum(q => q.QuestionPoints);
            if (theExam == null || theExam.InstructorID != userId)
            {
                TempData["Error"] = "Wrong credentials. Please try again";
                return View(addQuestionViewModel);
            }else if(theExam.ExamPoints < pointsSum + addQuestionViewModel.QuestionPoints)
            {
                TempData["Error"] = "The question points are larger than exam points";
                return View(addQuestionViewModel);
            }
            else
            {
                var newQuestion = new Question
                {
                    QuestionText = addQuestionViewModel.QuestionText,
                    QuestionPoints = addQuestionViewModel.QuestionPoints,
                    OptionsNumber = addQuestionViewModel.OptionsNumber,
                    ExamID = id,
                };

                _context.Questions.Add(newQuestion);
                _context.SaveChanges();
                return RedirectToAction("CurrentExams", "Instructor");
            }
        }

        public IActionResult AddOptions(int id)
        {
            var question = _context.Questions.FirstOrDefault(x => x.QuestionID == id);
            var options = new List<OptionViewModel>();
            for(int i = 0; i < question.OptionsNumber; i++)
            {
                var option = new OptionViewModel();
                options.Add(option);
            }
            var viewModel = new AddOptionsViewModel
            {
                options = options,
            };
            return View(viewModel);
        }

        [HttpPost]
        public async Task<IActionResult> AddOptions(int id, AddOptionsViewModel addOptionsViewModel)
        {
            if (!ModelState.IsValid) return View(addOptionsViewModel);
            if (id == null) return View("Error");
            var currentUserId = await _userManager.GetUserAsync(User);
            var userId = currentUserId?.Id;
            var theExam = _context.Exams.FirstOrDefault(x => x.ExamID == _context.Questions.FirstOrDefault(q => q.QuestionID == id).ExamID);
            var question = _context.Questions.FirstOrDefault(x => x.QuestionID == id);
            if (theExam == null || theExam.InstructorID != userId)
            {
                TempData["Error"] = "Wrong credentials. Please try again";
                return View(addOptionsViewModel);
            }else if(addOptionsViewModel.CorrectAnswer <= 0 || addOptionsViewModel.CorrectAnswer > question.OptionsNumber)
            {
                TempData["Error"] = $"The coorect answer must be between 1 and {question.OptionsNumber}";
                return View(addOptionsViewModel);
            }
            else if(addOptionsViewModel.options.Count < 2){
                TempData["Error"] = "Wrong credentials. Please try again";
                return View(addOptionsViewModel);
            }
            else if (_context.Options.Any(o => o.QuestionID == id))
            {
                TempData["Error"] = "Options for this question already exist.";
                return View(addOptionsViewModel);
            }
            else
            {
                int count = 1;
                foreach(var option in addOptionsViewModel.options)
                {
                    count++;
                    var newOption = new Option
                    {
                        OptionText = option.OptionText,
                        IsCorrect = count == addOptionsViewModel.CorrectAnswer ? true : false,
                        QuestionID = id,
                    };
                    _context.Options.Add(newOption);
                }
                _context.SaveChanges();
                return RedirectToAction("CurrentExams");
            }
        }

        public IActionResult Settings()
        {
            return View();
        }
    }
}
