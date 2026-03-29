namespace AIIntegratedCRM.Application.Features.Accounts.DTOs;

public class AccountDto
{
    public Guid Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public string? Industry { get; set; }
    public string? Website { get; set; }
    public string? Phone { get; set; }
    public string? Email { get; set; }
    public string? City { get; set; }
    public string? Country { get; set; }
    public decimal? AnnualRevenue { get; set; }
    public int? EmployeeCount { get; set; }
    public string? Description { get; set; }
    public Guid? AssignedToUserId { get; set; }
    public string? Tags { get; set; }
    public DateTime CreatedAt { get; set; }
    public int ContactCount { get; set; }
    public int OpportunityCount { get; set; }
}
