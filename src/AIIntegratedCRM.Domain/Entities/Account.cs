using AIIntegratedCRM.Domain.Common;

namespace AIIntegratedCRM.Domain.Entities;

public class Account : AuditableEntity
{
    public string Name { get; set; } = string.Empty;
    public string? Industry { get; set; }
    public string? Website { get; set; }
    public string? Phone { get; set; }
    public string? Email { get; set; }
    public string? Address { get; set; }
    public string? City { get; set; }
    public string? State { get; set; }
    public string? Country { get; set; }
    public string? PostalCode { get; set; }
    public decimal? AnnualRevenue { get; set; }
    public int? EmployeeCount { get; set; }
    public string? Description { get; set; }
    public Guid? AssignedToUserId { get; set; }
    public Guid? ParentAccountId { get; set; }
    public string? Tags { get; set; }
    public string? LinkedInUrl { get; set; }
    public string? TwitterHandle { get; set; }

    public Account? ParentAccount { get; set; }
    public ICollection<Contact> Contacts { get; set; } = new List<Contact>();
    public ICollection<Opportunity> Opportunities { get; set; } = new List<Opportunity>();
    public ICollection<SupportTicket> SupportTickets { get; set; } = new List<SupportTicket>();
    public ICollection<Activity> Activities { get; set; } = new List<Activity>();
}
