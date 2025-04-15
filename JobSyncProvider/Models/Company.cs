using JobSyncProvider.CustomValidations;
using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace JobSyncProvider.Models
{
	public class Company
	{ 
		public int CompanyId { get; set; }

        [DisplayName("Employer")]
        public string? EmployerId { get; set; }
        [DisplayName("Company Name")]
        public string Name { get; set; }

		//custom validation for a website
		[ValidWebsiteFormat]
		public string? Website { get; set; }

        public string Location { get; set; }

        [DisplayName("Year Founded")]
        public int FoundedYear { get; set; }
        public string Description { get; set; }
		public string? LogoUrl { get; set; }
        [DisplayName("Date Created")]
		public DateTime? DateCreated { get; set; }
		[DisplayName("Date Modified")]
		public DateTime? DateModified { get; set; }
		[DisplayName("Status")]
		public bool IsActive { get; set; }

		[ForeignKey("EmployerId")]
		[ValidateNever]
		public ApplicationUser Employer { get; set; }

		[ValidateNever]
		public ICollection<JobPost> JobPosts { get; set; }  // One company can have many jobs

	}
}
