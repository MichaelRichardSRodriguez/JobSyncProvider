using JobSyncProvider.DataAccess.Services;
using JobSyncProvider.Models;
using JobSyncProvider.Utilities;
using JobSyncProvider.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ValueGeneration.Internal;
using System.Security.Claims;

namespace JobSyncProvider.Areas.Admin.Controllers
{
	[Area("Admin")]
	[Authorize(Roles = StaticDetails.ROLE_ADMIN)]
	public class UserController : Controller
	{

		private readonly IUserService _userService;
		private readonly UserManager<IdentityUser> _userManager;
		private readonly RoleManager<IdentityRole> _roleManager;

		public UserController(IUserService userService, UserManager<IdentityUser> userManager, RoleManager<IdentityRole> roleManager)
		{
			_userService = userService;
			_userManager = userManager;
			_roleManager = roleManager;
		}

		// GET: UserController
		public IActionResult Index()
		{

			return View();

		}

		[HttpGet]
		public async Task<IActionResult> GetAllUsers([FromQuery] string selectedStatus,string selectedRole)
		{
			var users = await _userService.GetAllUsersAsync();

			// Filter user status
			if (selectedStatus?.ToUpper() != "ALL")
			{
				users = users.Where(c => c.Status == selectedStatus);
			}

			// Filter user role
            if (selectedRole?.ToUpper() != "ALL")
            {
                users = users.Where(c => _userManager.GetRolesAsync(c).Result.Contains(selectedRole));
            }

            var userVM = new List<UserVM>();

			foreach (var user in users)
			{
				// Get the roles for each user
				var roles = await _userManager.GetRolesAsync(user);
				var role = roles.FirstOrDefault(); // Single Role per User

				userVM.Add(new UserVM
				{
					Id = user.Id,
					FullName = user.FullName,
					Email = user.Email,
					Role = role,
					Status = user.Status
				});
			}

			return Json(new { data = userVM });

		}

		[HttpPost]
		[ValidateAntiForgeryToken]
		public async Task<IActionResult> ManageRole(UserVM userVM)
		{

			var claimsIdentity = (ClaimsIdentity)User.Identity;
			var userId = claimsIdentity.FindFirst(ClaimTypes.NameIdentifier).Value;

			//Check if Current User
			if (userVM.Id == userId)
			{
				//Initialize View Model
				var userViewModel = new UserVM
				{
					Id = userVM.Id,
					FullName = userVM.FullName,
					Email = userVM.Email,
					Role = userVM.Role,
					RoleList = _roleManager.Roles.Select(r => new SelectListItem
					{
						Text = r.Name,
						Value = r.Name
					})
				};

				TempData["error"] = "Not allowed to modify currently logged in account.";
				return View(userViewModel);
			}

			var applicationUser = await _userService.GetUserByIdAsync(userVM.Id);

			if (applicationUser == null)
			{
				return NotFound();
			}

			// Get Current Role
			var roles = await _userManager.GetRolesAsync(applicationUser);
			var currentRole = roles.FirstOrDefault();


			//Display message if No changes or Update is Successful
			if (currentRole == userVM.Role)
			{
				TempData["error"] = "No Changes Made.";
			}
			else
			{
				var newRole = await _roleManager.FindByNameAsync(userVM.Role); // Find the new role by name
				var newRoleId = newRole?.Id; // Get New RoleId

				await _userService.UpdateUserRoleAsync(userVM.Id, newRoleId);

				TempData["success"] = $"Updated role from {currentRole?.ToUpper()} to {userVM.Role.ToUpper()}.";
			}

			return RedirectToAction(nameof(Index));


		}


		public async Task<IActionResult> ManageRole(string id)
		{
			var user = await _userService.GetUserByIdAsync(id);
			var roles = await _userManager.GetRolesAsync(user);
			var role = roles.FirstOrDefault(); // Single Role per User

			var userVM = new UserVM()
			{
				Id = user.Id,
				FullName = user.FullName,
				Email = user.Email,
				Role = role,
				Status = user.Status,
				RoleList = _roleManager.Roles.Select(r => new SelectListItem
				{
					Text = r.Name,
					Value = r.Name
				})
			};

			return View(userVM);

		}

		[HttpPost]
		public async Task<IActionResult> UpdateUserStatus([FromBody] string id)
		{

			var claimsIdentity = (ClaimsIdentity)User.Identity;
			var userId = claimsIdentity.FindFirst(ClaimTypes.NameIdentifier).Value;

			if (id == userId)
			{
				return Json(new { success = false, message = "Not allowed to modify currently logged in account." });
			}

			var userFromDb = await _userService.GetUserByIdAsync(id);

			if (userFromDb == null)
			{
				return Json(new { success = false, message = "User not found." });
			}

			try
			{
				if (userFromDb.Status == StaticDetails.STATUS_VALID)
				{
					userFromDb.Status = StaticDetails.STATUS_INVALID;
				}
				else
				{
					userFromDb.Status = StaticDetails.STATUS_VALID;
				}

				await _userService.UpdateUserStatus(userFromDb);
			}
			catch (DbUpdateConcurrencyException)
			{

				TempData["error"] = "Update failed, due to concurrency issue. Please try again.";
				// Consider logging the exception for more details
				throw;
			}
			catch (Exception ex)
			{
				return Json(new { success = false, message = $"An unexpected error occurred: {ex.Message}." });
				// Log the exception for further investigation
			}

			return Json(new { success = true, message = "User Status successfully updated." });

		}


	}
}
