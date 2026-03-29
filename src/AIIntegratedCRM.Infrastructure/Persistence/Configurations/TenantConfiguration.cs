using AIIntegratedCRM.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace AIIntegratedCRM.Infrastructure.Persistence.Configurations;

public class TenantConfiguration : IEntityTypeConfiguration<Tenant>
{
    public void Configure(EntityTypeBuilder<Tenant> builder)
    {
        builder.ToTable("Tenants");
        builder.HasKey(t => t.Id);
        builder.Property(t => t.Name).IsRequired().HasMaxLength(200);
        builder.Property(t => t.Subdomain).IsRequired().HasMaxLength(50);
        builder.HasIndex(t => t.Subdomain).IsUnique();
        builder.Property(t => t.AdminEmail).IsRequired().HasMaxLength(255);
        builder.Property(t => t.Plan).HasMaxLength(50).HasDefaultValue("Starter");
        builder.Property(t => t.ConnectionString).HasMaxLength(1000);
    }
}
