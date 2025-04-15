using JobSyncProvider.Models;
using JobSyncProvider.Utilities;
using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel;

namespace JobSyncProvider.Configurations
{
	public class JobPostConfig : IEntityTypeConfiguration<JobPost>
	{
		public void Configure(EntityTypeBuilder<JobPost> builder)
		{
			//Primary Key
			builder.HasKey(j => j.JobPostId);

			builder.Property(a => a.Title).HasMaxLength(100).IsRequired();
			builder.Property(a => a.Description).HasColumnType("NVARCHAR(MAX)").IsRequired();
			builder.Property(a => a.WorkType).HasMaxLength(20).IsRequired();
			builder.Property(a => a.WorkSetup).HasMaxLength(20).IsRequired();
			builder.Property(a => a.Location).HasMaxLength(255).IsRequired();
			builder.Property(a => a.Salary).HasPrecision(10,2).IsRequired();

			// Setup default value for JobStatus
			builder.Property(a => a.JobStatus).HasDefaultValue(StaticDetails.STATUS_ACTIVE);

			// One to Many Relationship (Category to JobPost)
			builder.HasOne(c => c.Category).WithMany(j => j.JobPosts).HasForeignKey(c => c.CategoryId);

			// One to Many Relationship (Company to JobPost)
			builder.HasOne(c => c.Company).WithMany(j => j.JobPosts).HasForeignKey(c => c.CompanyId);

			// One to Many Relationship (Company to JobPost)
			builder.HasOne(c => c.Employer).WithMany().HasForeignKey(c => c.EmployerId);


		}
	}
}
