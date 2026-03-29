using AIIntegratedCRM.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace AIIntegratedCRM.Infrastructure.Persistence.Configurations;

public class ContactConfiguration : IEntityTypeConfiguration<Contact>
{
    public void Configure(EntityTypeBuilder<Contact> builder)
    {
        builder.ToTable("Contacts");
        builder.HasKey(c => c.Id);
        builder.Property(c => c.FirstName).IsRequired().HasMaxLength(100);
        builder.Property(c => c.LastName).IsRequired().HasMaxLength(100);
        builder.Property(c => c.Email).IsRequired().HasMaxLength(255);
        builder.Property(c => c.Phone).HasMaxLength(30);
        builder.Property(c => c.Mobile).HasMaxLength(30);
        builder.Property(c => c.Company).HasMaxLength(200);
        builder.Property(c => c.JobTitle).HasMaxLength(150);
        builder.Property(c => c.Notes).HasMaxLength(5000);
        builder.Property(c => c.Tags).HasMaxLength(500);
        builder.Property(c => c.Address).HasMaxLength(300);
        builder.Property(c => c.City).HasMaxLength(100);
        builder.Property(c => c.State).HasMaxLength(100);
        builder.Property(c => c.Country).HasMaxLength(100);
        builder.Property(c => c.PostalCode).HasMaxLength(20);
        builder.Property(c => c.LinkedInUrl).HasMaxLength(300);
        builder.Property(c => c.TwitterHandle).HasMaxLength(100);
        builder.Ignore(c => c.VectorEmbedding);

        builder.HasOne(c => c.Account).WithMany(a => a.Contacts).HasForeignKey(c => c.AccountId).OnDelete(DeleteBehavior.SetNull);

        builder.HasIndex(c => c.TenantId);
        builder.HasIndex(c => c.Email);
        builder.HasIndex(c => c.AccountId);
        builder.HasIndex(c => new { c.TenantId, c.IsDeleted });
        builder.HasQueryFilter(c => !c.IsDeleted);
    }
}
