using Microsoft.AspNetCore.Identity;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace JobSyncProvider.Models
{
	public class ApplicationUser : IdentityUser
	{

		[DisplayName("First Name")]
		[Required(ErrorMessage = "First Name is required.")]
		public string FirstName { get; set; }
		[DisplayName("Middle Name")]
		public string MiddleName { get; set; }
		[DisplayName("Last Name")]

		[Required(ErrorMessage = "Last Name is required.")]
		public string LastName { get; set; }
		public string Status { get; set; }

		[DisplayName("Name")]
		public string FullName { get; set; }

		//public string FullName
		//{

		//	get
		//	{
		//		var nameParts = new List<string>();

		//		if (!string.IsNullOrWhiteSpace(FirstName)) nameParts.Add(FirstName.Trim());
		//		if (!string.IsNullOrWhiteSpace(MiddleName)) nameParts.Add(MiddleName[0] + ".");
		//		if (!string.IsNullOrWhiteSpace(LastName)) nameParts.Add(LastName.Trim());

		//		return string.Join(" ", nameParts);

		//	}
		//}

	}
}
