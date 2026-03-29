using AIIntegratedCRM.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace AIIntegratedCRM.Infrastructure.Persistence.Configurations;

public class ActivityConfiguration : IEntityTypeConfiguration<Activity>
{
    public void Configure(EntityTypeBuilder<Activity> builder)
    {
        builder.ToTable("Activities");
        builder.HasKey(a => a.Id);
        builder.Property(a => a.Subject).IsRequired().HasMaxLength(200);
        builder.Property(a => a.Description).HasMaxLength(5000);
        builder.Property(a => a.Outcome).HasMaxLength(2000);
        builder.Property(a => a.AISummary).HasMaxLength(5000);
        builder.Property(a => a.Location).HasMaxLength(300);
        builder.Property(a => a.MeetingUrl).HasMaxLength(500);
        builder.Property(a => a.Tags).HasMaxLength(500);
        builder.Property(a => a.Direction).HasMaxLength(20);
        builder.Property(a => a.TranscriptText).HasColumnType("nvarchar(max)");

        builder.HasOne(a => a.Contact).WithMany(c => c.Activities).HasForeignKey(a => a.ContactId).OnDelete(DeleteBehavior.SetNull);
        builder.HasOne(a => a.Account).WithMany(ac => ac.Activities).HasForeignKey(a => a.AccountId).OnDelete(DeleteBehavior.SetNull);
        builder.HasOne(a => a.Opportunity).WithMany(o => o.Activities).HasForeignKey(a => a.OpportunityId).OnDelete(DeleteBehavior.SetNull);

        builder.HasIndex(a => a.TenantId);
        builder.HasIndex(a => a.StartTime);
        builder.HasIndex(a => a.ContactId);
        builder.HasIndex(a => new { a.TenantId, a.IsDeleted });
        builder.HasQueryFilter(a => !a.IsDeleted);
    }
}
