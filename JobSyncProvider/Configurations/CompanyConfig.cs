using JobSyncProvider.Models;
using JobSyncProvider.Utilities;
using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System.ComponentModel.DataAnnotations.Schema;

namespace JobSyncProvider.Configurations
{
    public class CompanyConfig : IEntityTypeConfiguration<Company>
    {
        public void Configure(EntityTypeBuilder<Company> builder)
        {
            builder.ToTable("Companies");

            //Primary Key
            builder.HasKey(c => c.CompanyId);

            builder.Property(c => c.Name).HasMaxLength(100).IsRequired();
            builder.Property(c => c.Website).HasMaxLength(100).IsRequired(false);
            builder.Property(c => c.Location).HasMaxLength(255).IsRequired();
            builder.Property(c => c.FoundedYear).IsRequired();
            builder.Property(c => c.Description).HasColumnType("NVARCHAR(MAX)").IsRequired();
            builder.Property(c => c.LogoUrl).HasColumnType("NVARCHAR(MAX)").IsRequired(false);
            builder.Property(c => c.IsActive).HasDefaultValue(StaticDetails.STATUS_ACTIVE);

            builder.HasOne(e => e.Employer).WithMany()
                    .HasForeignKey(e => e.EmployerId)
                    .IsRequired(false)
                    .OnDelete(DeleteBehavior.Cascade);

            builder.HasMany(j => j.JobPosts).WithOne(c => c.Company)
                    .HasForeignKey(e => e.CompanyId)
                    .IsRequired(false)
                    .OnDelete(DeleteBehavior.Cascade);

        }
    }
}
