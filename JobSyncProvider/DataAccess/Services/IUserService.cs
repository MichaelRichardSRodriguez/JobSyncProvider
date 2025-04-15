using JobSyncProvider.Models;
using Microsoft.AspNetCore.Identity;

namespace JobSyncProvider.DataAccess.Services
{
	public interface IUserService
	{
		Task<IEnumerable<ApplicationUser>> GetAllUsersAsync();
		Task<ApplicationUser> GetUserByIdAsync(string id);
		Task UpdateUserStatus(ApplicationUser applicationUser);
		Task UpdateUserRoleAsync(string oldRoleId, string newRoleId);

	}
}
