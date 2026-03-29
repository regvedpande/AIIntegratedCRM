using AIIntegratedCRM.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace AIIntegratedCRM.Infrastructure.Persistence.Configurations;

public class LeadConfiguration : IEntityTypeConfiguration<Lead>
{
    public void Configure(EntityTypeBuilder<Lead> builder)
    {
        builder.ToTable("Leads");
        builder.HasKey(l => l.Id);
        builder.Property(l => l.FirstName).IsRequired().HasMaxLength(100);
        builder.Property(l => l.LastName).IsRequired().HasMaxLength(100);
        builder.Property(l => l.Email).IsRequired().HasMaxLength(255);
        builder.Property(l => l.Phone).HasMaxLength(30);
        builder.Property(l => l.Company).HasMaxLength(200);
        builder.Property(l => l.JobTitle).HasMaxLength(150);
        builder.Property(l => l.Notes).HasMaxLength(5000);
        builder.Property(l => l.Tags).HasMaxLength(500);
        builder.Property(l => l.Website).HasMaxLength(300);
        builder.Property(l => l.LinkedInUrl).HasMaxLength(300);
        builder.Property(l => l.Country).HasMaxLength(100);
        builder.Property(l => l.City).HasMaxLength(100);
        builder.Property(l => l.AIScore).HasPrecision(5, 2);
        builder.Property(l => l.AIScoreReason).HasMaxLength(2000);

        builder.HasIndex(l => l.TenantId);
        builder.HasIndex(l => l.Email);
        builder.HasIndex(l => l.Status);
        builder.HasIndex(l => new { l.TenantId, l.IsDeleted });
        builder.HasQueryFilter(l => !l.IsDeleted);
    }
}
