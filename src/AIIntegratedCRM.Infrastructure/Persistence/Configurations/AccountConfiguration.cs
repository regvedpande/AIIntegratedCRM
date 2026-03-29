using AIIntegratedCRM.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace AIIntegratedCRM.Infrastructure.Persistence.Configurations;

public class AccountConfiguration : IEntityTypeConfiguration<Account>
{
    public void Configure(EntityTypeBuilder<Account> builder)
    {
        builder.ToTable("Accounts");
        builder.HasKey(a => a.Id);
        builder.Property(a => a.Name).IsRequired().HasMaxLength(200);
        builder.Property(a => a.Industry).HasMaxLength(100);
        builder.Property(a => a.Website).HasMaxLength(300);
        builder.Property(a => a.Phone).HasMaxLength(30);
        builder.Property(a => a.Email).HasMaxLength(255);
        builder.Property(a => a.Address).HasMaxLength(300);
        builder.Property(a => a.City).HasMaxLength(100);
        builder.Property(a => a.State).HasMaxLength(100);
        builder.Property(a => a.Country).HasMaxLength(100);
        builder.Property(a => a.PostalCode).HasMaxLength(20);
        builder.Property(a => a.Description).HasMaxLength(5000);
        builder.Property(a => a.Tags).HasMaxLength(500);
        builder.Property(a => a.AnnualRevenue).HasPrecision(18, 2);

        builder.HasOne(a => a.ParentAccount).WithMany().HasForeignKey(a => a.ParentAccountId).OnDelete(DeleteBehavior.SetNull);

        builder.HasIndex(a => a.TenantId);
        builder.HasIndex(a => a.Name);
        builder.HasIndex(a => new { a.TenantId, a.IsDeleted });
        builder.HasQueryFilter(a => !a.IsDeleted);
    }
}
