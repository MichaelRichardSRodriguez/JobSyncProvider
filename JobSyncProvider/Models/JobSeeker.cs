using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using System.ComponentModel.DataAnnotations.Schema;

namespace JobSyncProvider.Models
{
	public class JobSeeker
	{
		public int JobSeekerId { get; set; }
		public string UserId { get; set; }
		public string Resume {  get; set; }
		public string Skills { get; set; }

		[ForeignKey("UserId")]
		[ValidateNever]
		public ApplicationUser Users { get; set; }	
	}
}
