using JobSyncProvider.Models;
using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System.ComponentModel.DataAnnotations.Schema;

namespace JobSyncProvider.Configurations
{
	public class CategoryConfig : IEntityTypeConfiguration<Category>
	{
		public void Configure(EntityTypeBuilder<Category> builder)
		{
			//Table Configuration
			builder.ToTable("Categories");

			//Primary Key
			builder.HasKey(c => c.CategoryId);

			//Columns
			builder.Property(c => c.Name)
					.IsRequired()
					.HasMaxLength(100);

			builder.Property(c => c.DateCreated)
					.IsRequired(false);

			builder.HasOne(c => c.CreatedByUser)
				.WithMany()
				.HasForeignKey(c => c.CreatedBy)
				.OnDelete(DeleteBehavior.SetNull);

			builder.HasOne(c => c.ModifiedByUser)
					.WithMany()
					.HasForeignKey(c => c.ModifiedBy)
					.OnDelete(DeleteBehavior.SetNull);

			builder.HasMany(c => c.JobPosts)
					.WithOne(j => j.Category)
					.HasForeignKey(j => j.CategoryId)
					.OnDelete(DeleteBehavior.NoAction);

		}

	}
}
