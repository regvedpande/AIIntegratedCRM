using AIIntegratedCRM.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace AIIntegratedCRM.Infrastructure.Persistence.Configurations;

public class OpportunityConfiguration : IEntityTypeConfiguration<Opportunity>
{
    public void Configure(EntityTypeBuilder<Opportunity> builder)
    {
        builder.ToTable("Opportunities");
        builder.HasKey(o => o.Id);
        builder.Property(o => o.Name).IsRequired().HasMaxLength(200);
        builder.Property(o => o.Amount).HasPrecision(18, 2);
        builder.Property(o => o.Currency).HasMaxLength(3).HasDefaultValue("USD");
        builder.Property(o => o.Probability).HasPrecision(5, 2);
        builder.Property(o => o.AIProbability).HasPrecision(5, 2);
        builder.Property(o => o.AIProbabilityReason).HasMaxLength(2000);
        builder.Property(o => o.Description).HasMaxLength(5000);
        builder.Property(o => o.LostReason).HasMaxLength(1000);
        builder.Property(o => o.CompetitorInfo).HasMaxLength(2000);
        builder.Property(o => o.NextStep).HasMaxLength(1000);
        builder.Property(o => o.Tags).HasMaxLength(500);
        builder.Property(o => o.LeadSource).HasMaxLength(100);

        builder.HasOne(o => o.Account).WithMany(a => a.Opportunities).HasForeignKey(o => o.AccountId).OnDelete(DeleteBehavior.SetNull);
        builder.HasOne(o => o.PrimaryContact).WithMany(c => c.Opportunities).HasForeignKey(o => o.PrimaryContactId).OnDelete(DeleteBehavior.SetNull);

        builder.HasIndex(o => o.TenantId);
        builder.HasIndex(o => o.Stage);
        builder.HasIndex(o => o.AccountId);
        builder.HasIndex(o => new { o.TenantId, o.IsDeleted });
        builder.HasQueryFilter(o => !o.IsDeleted);
    }
}
