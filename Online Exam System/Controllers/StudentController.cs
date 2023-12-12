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
        public IActionResult ExamsHistory()
        {
            return View();  
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
