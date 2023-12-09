using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Online_Exam_System.Data;
using Online_Exam_System.Models;
using Online_Exam_System.ViewModels;

namespace Online_Exam_System.Controllers
{
	[Authorize(Roles = "Admin")]
	public class AdminController : Controller
	{
		private readonly UserManager<User> _userManager;
		private readonly RoleManager<IdentityRole> _roleManager;
		private readonly SignInManager<User> _signInManager;
		private readonly ApplicationDbContext _context;

		public AdminController(UserManager<User> userManager, SignInManager<User> signInManager, ApplicationDbContext context, RoleManager<IdentityRole> roleManager)
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

		public IActionResult CreateUser()
		{
			var response = new CreateUserViewModel();
			return View(response);
		}

		[HttpPost]
		public async Task<IActionResult> CreateUser(CreateUserViewModel createUserViewModel)
		{
			if (!ModelState.IsValid) return View(createUserViewModel);
			var user = await _userManager.FindByEmailAsync(createUserViewModel.EmailAddress);
			if (user != null)
			{
				TempData["Error"] = "This email address is already in use";
				return View(createUserViewModel);
			}

			var newUser = new User()
			{
				Email = createUserViewModel.EmailAddress,
				UserName = createUserViewModel.EmailAddress,
				name = createUserViewModel.Name,
				role = createUserViewModel.Role,
			};
			var newUserResponse = await _userManager.CreateAsync(newUser, createUserViewModel.Password);
			if (!newUserResponse.Succeeded)
			{
				TempData["Error"] = newUserResponse.Errors.First().Description;
				return View(createUserViewModel);
			}
			else
			{
				// Process profile picture
				if (createUserViewModel.ProfilePicture != null && createUserViewModel.ProfilePicture.Length > 0)
				{
					using (var memoryStream = new MemoryStream())
					{
						await createUserViewModel.ProfilePicture.CopyToAsync(memoryStream);
						newUser.ProfilePictureData = memoryStream.ToArray();
					}
				}

				// Check if the role exists
				var roleExists = await _roleManager.RoleExistsAsync(createUserViewModel.Role);

				// If it doesn't exist, create it
				if (!roleExists)
				{
					await _roleManager.CreateAsync(new IdentityRole(createUserViewModel.Role));
				}

				//add the user to the "student" role
				await _userManager.AddToRoleAsync(newUser, createUserViewModel.Role);

			}

			return RedirectToAction("Index", "Admin");
		}

		public IActionResult AllUsers()
		{
			var users = _context.Users.ToList();
			return View(users);
		}

		public IActionResult DeleteUser(string id)
		{
			var user = _context.Users.FirstOrDefault(x => x.Id == id);
			if (user == null) return View("Error");
			_context.Users.Remove(user);
			_context.SaveChanges();
			return RedirectToAction("Index");
		}

		public IActionResult EditUser(string id)
		{
			var user = _context.Users.FirstOrDefault(x => x.Id == id);
			if (user == null) return View("Error");
			var editUserVM = new EditUserViewModel()
			{
				Name = user.name,
				EmailAddress = user.Email,
				Password = "",
				ConfirmPassword = "",
				Role = user.role,
			};
			return View(editUserVM);
		}

		[HttpPost]
		public async Task<IActionResult> EditUser(string id, EditUserViewModel editUserViewModel)
		{
			if (!ModelState.IsValid)
			{
				ModelState.AddModelError("", "Failed to edit");
				return View("EditUser", editUserViewModel);
			}
			var user = _context.Users.FirstOrDefault(x => x.Id == id);
			if (user != null)
			{
				user.name = editUserViewModel.Name;
				user.Email = editUserViewModel.EmailAddress;
				user.role = editUserViewModel.Role;
				await _userManager.RemovePasswordAsync(user);
				await _userManager.AddPasswordAsync(user, editUserViewModel.Password);
				var editedUserResponse = await _userManager.UpdateAsync(user);
				return RedirectToAction("AllUsers", "Admin");
			}
			else
			{
				return View(editUserViewModel);
			}
		}
		public IActionResult Settings(string id)
		{
			var user = _context.Users.FirstOrDefault(x => x.Id == id);
			if (user == null) return View("Error");
			var editUserVM = new AdminSettingsViewModel()
			{
				Name = user.name,
				EmailAddress = user.Email,
				Password = "",
				ConfirmPassword = "",
			};
			return View(editUserVM);
		}

		[HttpPost]
		public async Task<IActionResult> Settings(string id, AdminSettingsViewModel adminSettingsViewModel)
		{
			if (!ModelState.IsValid)
			{
				ModelState.AddModelError("", "Failed to edit");
				return View("Settings", adminSettingsViewModel);
			}
			var user = _context.Users.FirstOrDefault(x => x.Id == id);
			if (user != null)
			{
				user.name = adminSettingsViewModel.Name;
				user.Email = adminSettingsViewModel.EmailAddress;
				await _userManager.RemovePasswordAsync(user);
				await _userManager.AddPasswordAsync(user, adminSettingsViewModel.Password);
				var editedUserResponse = await _userManager.UpdateAsync(user);
				return RedirectToAction("Index", "Admin");
			}
			else
			{
				return View(adminSettingsViewModel);
			}
		}
	}
}