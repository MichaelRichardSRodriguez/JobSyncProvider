using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations.Schema;

namespace JobSyncProvider.Models
{
	public class JobPost
	{
		public int JobPostId { get; set; }
		[DisplayName("Title / Position")]
		public string Title { get; set; }
		public string Description { get; set; }
		[DisplayName("Work Type")]
		public string WorkType { get; set; }
		[DisplayName("Setup")]
		public string WorkSetup { get; set; }
		public string Location { get; set; }
		public decimal Salary { get; set; }

        [DisplayName("User")]
        public string? EmployerId { get; set; }
		[DisplayName("Category")]
		public int CategoryId { get; set; }
        [DisplayName("Company")]
        public int? CompanyId { get; set; }
		public DateTime? DatePosted { get; set; }
		public DateTime? DateModified { get; set; }
		public bool? JobStatus { get; set; }

		[ForeignKey("EmployerId")]
        [ValidateNever]
        public  ApplicationUser Employer { get; set; }

		[ForeignKey("CategoryId")]
		[ValidateNever]

		public Category Category { get; set; }

        [ForeignKey("CompanyId")]
        [ValidateNever]

        public Company Company { get; set; }

    }
}
