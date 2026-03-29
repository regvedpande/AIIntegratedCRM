using AIIntegratedCRM.Domain.Common;

namespace AIIntegratedCRM.Domain.Entities;

public class Contact : AuditableEntity
{
    public string FirstName { get; set; } = string.Empty;
    public string LastName { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;
    public string? Phone { get; set; }
    public string? Mobile { get; set; }
    public string? Company { get; set; }
    public string? JobTitle { get; set; }
    public Guid? AccountId { get; set; }
    public Guid? AssignedToUserId { get; set; }
    public string? Notes { get; set; }
    public string? Tags { get; set; }
    public string? Address { get; set; }
    public string? City { get; set; }
    public string? State { get; set; }
    public string? Country { get; set; }
    public string? PostalCode { get; set; }
    public string? LinkedInUrl { get; set; }
    public string? TwitterHandle { get; set; }
    public DateTime? BirthDate { get; set; }
    public float[]? VectorEmbedding { get; set; }
    public bool DoNotContact { get; set; } = false;
    public DateTime? LastContactedAt { get; set; }

    public string FullName => $"{FirstName} {LastName}".Trim();

    public Account? Account { get; set; }
    public ICollection<Activity> Activities { get; set; } = new List<Activity>();
    public ICollection<Opportunity> Opportunities { get; set; } = new List<Opportunity>();
    public ICollection<SupportTicket> SupportTickets { get; set; } = new List<SupportTicket>();
}
