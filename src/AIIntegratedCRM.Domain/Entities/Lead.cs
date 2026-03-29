using AIIntegratedCRM.Domain.Common;
using AIIntegratedCRM.Domain.Enums;
using AIIntegratedCRM.Domain.Events;

namespace AIIntegratedCRM.Domain.Entities;

public class Lead : AuditableEntity
{
    public string FirstName { get; set; } = string.Empty;
    public string LastName { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;
    public string? Phone { get; set; }
    public string? Company { get; set; }
    public string? JobTitle { get; set; }
    public LeadSource Source { get; set; } = LeadSource.Website;
    public LeadStatus Status { get; set; } = LeadStatus.New;
    public decimal AIScore { get; set; } = 0;
    public string? AIScoreReason { get; set; }
    public string? Notes { get; set; }
    public Guid? AssignedToUserId { get; set; }
    public DateTime? LastContactedAt { get; set; }
    public DateTime? ConvertedAt { get; set; }
    public Guid? ConvertedContactId { get; set; }
    public Guid? ConvertedAccountId { get; set; }
    public Guid? ConvertedOpportunityId { get; set; }
    public string? Tags { get; set; }
    public string? Website { get; set; }
    public string? LinkedInUrl { get; set; }
    public int? AnnualRevenue { get; set; }
    public int? EmployeeCount { get; set; }
    public string? Country { get; set; }
    public string? City { get; set; }

    public string FullName => $"{FirstName} {LastName}".Trim();

    public static Lead Create(string firstName, string lastName, string email, Guid tenantId, string createdBy)
    {
        var lead = new Lead
        {
            FirstName = firstName,
            LastName = lastName,
            Email = email,
            TenantId = tenantId,
            CreatedBy = createdBy
        };
        lead.AddDomainEvent(new LeadCreatedEvent(lead.Id, tenantId, email));
        return lead;
    }

    public void Convert(Guid contactId, Guid accountId, Guid opportunityId)
    {
        Status = LeadStatus.Converted;
        ConvertedAt = DateTime.UtcNow;
        ConvertedContactId = contactId;
        ConvertedAccountId = accountId;
        ConvertedOpportunityId = opportunityId;
    }
}
