using AIIntegratedCRM.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace AIIntegratedCRM.Application.Common.Interfaces;

public interface IApplicationDbContext
{
    DbSet<Tenant> Tenants { get; }
    DbSet<AppUser> Users { get; }
    DbSet<Lead> Leads { get; }
    DbSet<Contact> Contacts { get; }
    DbSet<Account> Accounts { get; }
    DbSet<Opportunity> Opportunities { get; }
    DbSet<Activity> Activities { get; }
    DbSet<SupportTicket> SupportTickets { get; }
    DbSet<TicketComment> TicketComments { get; }
    DbSet<WorkflowRule> WorkflowRules { get; }

    Task<int> SaveChangesAsync(CancellationToken cancellationToken = default);
}
