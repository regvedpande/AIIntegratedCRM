using AIIntegratedCRM.Domain.Enums;

namespace AIIntegratedCRM.Application.Features.Leads.DTOs;

public class LeadDto
{
    public Guid Id { get; set; }
    public string FullName { get; set; } = string.Empty;
    public string FirstName { get; set; } = string.Empty;
    public string LastName { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;
    public string? Phone { get; set; }
    public string? Company { get; set; }
    public string? JobTitle { get; set; }
    public LeadSource Source { get; set; }
    public LeadStatus Status { get; set; }
    public decimal AIScore { get; set; }
    public string? AIScoreReason { get; set; }
    public string? Notes { get; set; }
    public Guid? AssignedToUserId { get; set; }
    public string? Tags { get; set; }
    public string? Country { get; set; }
    public string? City { get; set; }
    public int? EmployeeCount { get; set; }
    public int? AnnualRevenue { get; set; }
    public DateTime CreatedAt { get; set; }
    public DateTime? ConvertedAt { get; set; }
    public bool IsConverted => ConvertedAt.HasValue;
}
