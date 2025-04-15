using JobSyncProvider.DataAccess.Repository;
using JobSyncProvider.DataAccess.UnitOfWork;
using JobSyncProvider.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace JobSyncProvider.DataAccess.Services
{
	public class UserService : IUserService
	{
		private readonly IUnitOfWork _unitOfWork;
		private readonly IRepository<ApplicationUser> _userRepository;
		private readonly RoleManager<IdentityRole> _roleManager;
		private readonly UserManager<IdentityUser> _userManager;

		public UserService(IUnitOfWork unitOfWork,
				UserManager<IdentityUser> userManager,
				RoleManager<IdentityRole> roleManager)
		{
			_unitOfWork = unitOfWork;
			_userManager = userManager;	
			_roleManager = roleManager;
			_userRepository = _unitOfWork.GetRepository<ApplicationUser>();
        }

		public async Task<IEnumerable<ApplicationUser>> GetAllUsersAsync()
		{

			return await _userRepository.GetAllRecordsAsync();
			
		}

		public async Task<ApplicationUser> GetUserByIdAsync(string id)
		{

			return await _userRepository.GetRecordAsync(a => a.Id == id);

		}

		public async Task UpdateUserRoleAsync(string oldRoleId,string newRoleId)
		{

			// Get the role by RoleId
			var role = await _roleManager.FindByIdAsync(newRoleId);

			if (role == null)
			{
				throw new Exception("Role not found");
			}

			var user = await _userManager.FindByIdAsync(oldRoleId);
			var currentRoles = await _userManager.GetRolesAsync(user);

			var removeResult = await _userManager.RemoveFromRolesAsync(user, currentRoles);

			if (!removeResult.Succeeded)
			{
				throw new Exception("Error Removing Current Role");
			}

			await _userManager.AddToRoleAsync(user, role.Name);

		}

		public async Task UpdateUserStatus(ApplicationUser applicationUser)
		{
			_userRepository.UpdateRecord(applicationUser);
			await _unitOfWork.CommitAsync();
		}
	}
}
