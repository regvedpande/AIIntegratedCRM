using AIIntegratedCRM.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace AIIntegratedCRM.Infrastructure.Persistence.Configurations;

public class WorkflowRuleConfiguration : IEntityTypeConfiguration<WorkflowRule>
{
    public void Configure(EntityTypeBuilder<WorkflowRule> builder)
    {
        builder.ToTable("WorkflowRules");
        builder.HasKey(w => w.Id);
        builder.Property(w => w.Name).IsRequired().HasMaxLength(200);
        builder.Property(w => w.Description).HasMaxLength(1000);
        builder.Property(w => w.ConditionsJson).HasColumnType("nvarchar(max)").HasDefaultValue("{}");
        builder.Property(w => w.ActionsJson).HasColumnType("nvarchar(max)").HasDefaultValue("[]");

        builder.HasIndex(w => w.TenantId);
        builder.HasIndex(w => new { w.TenantId, w.IsDeleted });
        builder.HasQueryFilter(w => !w.IsDeleted);
    }
}
