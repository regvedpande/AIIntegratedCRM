using AIIntegratedCRM.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace AIIntegratedCRM.Infrastructure.Persistence.Configurations;

public class SupportTicketConfiguration : IEntityTypeConfiguration<SupportTicket>
{
    public void Configure(EntityTypeBuilder<SupportTicket> builder)
    {
        builder.ToTable("SupportTickets");
        builder.HasKey(t => t.Id);
        builder.Property(t => t.Subject).IsRequired().HasMaxLength(300);
        builder.Property(t => t.Description).IsRequired().HasMaxLength(5000);
        builder.Property(t => t.Resolution).HasMaxLength(5000);
        builder.Property(t => t.Tags).HasMaxLength(500);
        builder.Property(t => t.Category).HasMaxLength(100);

        builder.HasOne(t => t.Contact).WithMany(c => c.SupportTickets).HasForeignKey(t => t.ContactId).OnDelete(DeleteBehavior.SetNull);
        builder.HasOne(t => t.Account).WithMany(a => a.SupportTickets).HasForeignKey(t => t.AccountId).OnDelete(DeleteBehavior.SetNull);
        builder.HasMany(t => t.Comments).WithOne(c => c.Ticket).HasForeignKey(c => c.TicketId).OnDelete(DeleteBehavior.Cascade);

        builder.HasIndex(t => t.TenantId);
        builder.HasIndex(t => t.Status);
        builder.HasIndex(t => t.Priority);
        builder.HasIndex(t => new { t.TenantId, t.IsDeleted });
        builder.HasQueryFilter(t => !t.IsDeleted);
    }
}
