using JobSyncProvider.Configurations;
using JobSyncProvider.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace JobSyncProvider.DataAccess.Data
{
	public class ApplicationDbContext : IdentityDbContext<ApplicationUser>
	{

		public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
		{

		}

		public DbSet<JobPost> JobPosts { get; set; }
		public DbSet<JobSeeker> JobSeekers { get; set; }
		public DbSet<JobApplication> JobApplications { get; set; }
		public DbSet<Category> Categories { get; set; }
		public DbSet<Company> Companies { get; set; }
		public DbSet<ApplicationUser> ApplicationUsers { get; set; }

		protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
		{
			//base.OnConfiguring(optionsBuilder);
		}

		protected override void OnModelCreating(ModelBuilder builder)
		{

			base.OnModelCreating(builder);

			builder.ApplyConfiguration(new ApplicationUserConfig());
			builder.ApplyConfiguration(new CategoryConfig());
			builder.ApplyConfiguration(new CompanyConfig());
			builder.ApplyConfiguration(new JobPostConfig());

		}


	}
}
