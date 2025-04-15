using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace JobSyncProvider.Models
{
	public class JobApplication
	{
		[Key]
		public int ApplicationId { get; set; }
		public int JobPostId { get; set; }
		public int JobSeekerId { get; set; }
		public DateTime ApplicationDate { get; set; }
		public string JobStatus { get; set; }

		[ForeignKey("JobPostId")]
		[ValidateNever]
		public JobPost JobPost { get; set; }

		[ForeignKey("JobSeekerId")]

		[ValidateNever]
		public JobSeeker JobSeeker { get; set; }

	}
}
