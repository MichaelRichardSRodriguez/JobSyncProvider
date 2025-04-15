using JobSyncProvider.Models;
using JobSyncProvider.Utilities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace JobSyncProvider.Configurations
{
	public class ApplicationUserConfig : IEntityTypeConfiguration<ApplicationUser>
	{
		public void Configure(EntityTypeBuilder<ApplicationUser> builder)
		{
			builder.Property(a => a.FirstName).HasMaxLength(50).IsRequired();
			builder.Property(a => a.MiddleName).HasMaxLength(50).IsRequired();
			builder.Property(a => a.LastName).HasMaxLength(50).IsRequired();
			builder.Property(a => a.FullName).HasMaxLength(150).IsRequired();
			builder.Property(a => a.Status).HasDefaultValue(StaticDetails.STATUS_VALID);
		}
	}
}
