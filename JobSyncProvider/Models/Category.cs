using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations.Schema;
using System.Diagnostics.Contracts;
using System.Windows.Markup;

namespace JobSyncProvider.Models
{
	public class Category
	{

		public int CategoryId { get; set; }
        [DisplayName("Category Name")]
        public string Name { get; set; }
		[DisplayName("Date Created")]
		public DateTime? DateCreated { get; set; }
        [DisplayName("Date Updated")]
        public DateTime? DateUpdated { get; set; }

        [DisplayName("Created By")]
        public string? CreatedBy { get; set; }

        [DisplayName("Modified By")]
        public string? ModifiedBy {  get; set; }
		public string? Status { get; set; }

		[ForeignKey("CreatedBy")]
		[ValidateNever]
		public ApplicationUser CreatedByUser { get; set; }

		[ForeignKey("ModifiedBy")]
        [ValidateNever]
        public ApplicationUser ModifiedByUser { get; set; }

		[ValidateNever]
		public ICollection<JobPost> JobPosts { get; set; }  // One category can have many jobs
	}
}
