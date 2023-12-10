﻿using Microsoft.AspNetCore.Authorization;
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
            var viewModel = new CurrentExamsViewModel()
            {
                Exams = userExams,
                ExamQuestionCounts = examQuestionCounts,
                ExamQuestionsPointSum = examQuestionsPointsSum,
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

        public IActionResult Settings()
        {
            return View();
        }
    }
}
