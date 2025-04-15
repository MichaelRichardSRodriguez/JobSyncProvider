using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace JobSyncProvider.ViewModels
{
	public class UserVM
	{

		public string Id { get; set; }
		public string FullName { get; set; }
		public string Email { get; set; }
		public string Role { get; set; }
		public string Status { get; set; }

		[ValidateNever]
		public IEnumerable<SelectListItem> RoleList { get; set; }
	}
}
