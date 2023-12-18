using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Online_Exam_System.Data;
using Online_Exam_System.Models;
using Online_Exam_System.ViewModels;

namespace Online_Exam_System.Controllers
{
    public class AccountController : Controller
    {
        private readonly UserManager<User> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly SignInManager<User> _signInManager;
        private readonly ApplicationDbContext _context;

        public AccountController(UserManager<User> userManager, SignInManager<User> signInManager, ApplicationDbContext context, RoleManager<IdentityRole> roleManager)
        {
            _userManager = userManager;
            _roleManager = roleManager;
            _signInManager = signInManager;
            _context = context;
        }

        public IActionResult Login()
        {
            var response = new LoginViewModel();
            return View(response);
        }

        [HttpPost]
        public async Task<IActionResult> Login(LoginViewModel loginViewModel)
        {
            if(!ModelState.IsValid) return View(loginViewModel);

            var user = await _userManager.FindByEmailAsync(loginViewModel.EmailAddress);
            if(user != null)
            {
                // User is found
                var PasswordCheck = await _userManager.CheckPasswordAsync(user, loginViewModel.Password);
                if(PasswordCheck)
                {
                    // Password is correct sign in
                    var result = await _signInManager.PasswordSignInAsync(user, loginViewModel.Password, false, false);
                    if(result.Succeeded)
                    {
                        return RedirectToAction("Index", "Home");
                    }
                }
                // Password is incorrect
                TempData["Error"] = "Wrong credentials. Please try again";
                return View(loginViewModel);
            }
            // User not found
            TempData["Error"] = "Wrong credentials. Please try again";
            return View(loginViewModel);
        }
        
        public IActionResult Register()
        {
            var response = new RegisterViewModel();
            return View(response);
        }

        [HttpPost]
        public async Task<IActionResult> Register(RegisterViewModel registerViewModel)
        {
            if(!ModelState.IsValid) return View(registerViewModel);
            var user = await _userManager.FindByEmailAsync(registerViewModel.EmailAddress);
            if(user != null)
            {
                TempData["Error"] = "This email address is already in use";
                return View(registerViewModel);
            }

            var newUser = new User()
            {
                Email = registerViewModel.EmailAddress,
                UserName = registerViewModel.EmailAddress,
                Role = Roles.student,
                Name = registerViewModel.Name,
            };
            var newUserResponse = await _userManager.CreateAsync(newUser, registerViewModel.Password);
            if(!newUserResponse.Succeeded)
            {
                TempData["Error"] = newUserResponse.Errors.First().Description;
                return View(registerViewModel);
            }
            else
            {
                // Process profile picture
                if (registerViewModel.ProfilePicture != null && registerViewModel.ProfilePicture.Length > 0)
                {
                    using (var memoryStream = new MemoryStream())
                    {
                        await registerViewModel.ProfilePicture.CopyToAsync(memoryStream);
                        newUser.ProfilePictureData = memoryStream.ToArray();
                    }
                }

                // Check if the role exists
                var roleExists = await _roleManager.RoleExistsAsync(Roles.student);

                // If it doesn't exist, create it
                if (!roleExists)
                {
                    await _roleManager.CreateAsync(new IdentityRole(Roles.student));
                }

                //add the user to the "student" role
                await _userManager.AddToRoleAsync(newUser, Roles.student);

            }

            return RedirectToAction("Index", "Home");
        }

        [HttpGet]
        public async Task<IActionResult> Logout()
        {
            await _signInManager.SignOutAsync();
            return RedirectToAction("Login", "Account");
        }


        [HttpGet]
        public IActionResult GetProfilePicture(string userId)
        {
            var user = _userManager.FindByIdAsync(userId).Result;

            if (user != null && user.ProfilePictureData != null)
            {
                return File(user.ProfilePictureData, "image/jpeg"); // Adjust the content type based on your image format
            }

            // If no image is found, return a default image or a placeholder
            return File("~/path/to/default/image.jpg", "image/jpeg"); // Adjust the path and content type accordingly
        }
    }
}
