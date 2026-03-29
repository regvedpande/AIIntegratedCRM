using System.Text.Json.Serialization;

namespace AIIntegratedCRM.Blazor.Models;

public class PaginatedResult<T>
{
    public List<T> Items { get; set; } = new();
    public int TotalCount { get; set; }
    public int PageNumber { get; set; }
    public int PageSize { get; set; }
    public int TotalPages { get; set; }
    public bool HasNextPage { get; set; }
    public bool HasPreviousPage { get; set; }
}

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
    public string Source { get; set; } = string.Empty;
    public string Status { get; set; } = string.Empty;
    public decimal AIScore { get; set; }
    public string? AIScoreReason { get; set; }
    public string? Notes { get; set; }
    public string? Tags { get; set; }
    public string? Country { get; set; }
    public DateTime CreatedAt { get; set; }
    public bool IsConverted { get; set; }
}

public class ContactDto
{
    public Guid Id { get; set; }
    public string FullName { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;
    public string? Phone { get; set; }
    public string? Company { get; set; }
    public string? JobTitle { get; set; }
    public Guid? AccountId { get; set; }
    public string? AccountName { get; set; }
    public string? City { get; set; }
    public string? Country { get; set; }
    public DateTime CreatedAt { get; set; }
}

public class AccountDto
{
    public Guid Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public string? Industry { get; set; }
    public string? Website { get; set; }
    public string? Phone { get; set; }
    public string? City { get; set; }
    public string? Country { get; set; }
    public decimal? AnnualRevenue { get; set; }
    public int? EmployeeCount { get; set; }
    public DateTime CreatedAt { get; set; }
}

public class OpportunityDto
{
    public Guid Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public Guid? AccountId { get; set; }
    public string? AccountName { get; set; }
    public string Stage { get; set; } = string.Empty;
    public decimal Amount { get; set; }
    public string Currency { get; set; } = "USD";
    public decimal Probability { get; set; }
    public decimal AIProbability { get; set; }
    public string? AIProbabilityReason { get; set; }
    public DateTime? ExpectedCloseDate { get; set; }
    public string? NextStep { get; set; }
    public DateTime CreatedAt { get; set; }
    public decimal WeightedAmount { get; set; }
}

public class ActivityDto
{
    public Guid Id { get; set; }
    public string Type { get; set; } = string.Empty;
    public string Subject { get; set; } = string.Empty;
    public string? Description { get; set; }
    public DateTime StartTime { get; set; }
    public string? ContactName { get; set; }
    public string? AccountName { get; set; }
    public bool IsCompleted { get; set; }
    public string? AISummary { get; set; }
    public DateTime CreatedAt { get; set; }
}

public class SupportTicketDto
{
    public Guid Id { get; set; }
    public string Subject { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public string Status { get; set; } = string.Empty;
    public string Priority { get; set; } = string.Empty;
    public string? ContactName { get; set; }
    public string? AccountName { get; set; }
    public DateTime CreatedAt { get; set; }
    public DateTime? ResolvedAt { get; set; }
}

public class DashboardStats
{
    public int TotalLeads { get; set; }
    public int NewLeads { get; set; }
    public int TotalContacts { get; set; }
    public int TotalAccounts { get; set; }
    public int TotalOpportunities { get; set; }
    public int OpenOpportunities { get; set; }
    public decimal TotalPipelineValue { get; set; }
    public int WonOpportunities { get; set; }
    public int TotalTickets { get; set; }
    public int OpenTickets { get; set; }
    public List<RecentLead> RecentLeads { get; set; } = new();
    public List<PipelineStage> PipelineByStage { get; set; } = new();
}

public class RecentLead
{
    public Guid Id { get; set; }
    public string FirstName { get; set; } = string.Empty;
    public string LastName { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;
    public string? Company { get; set; }
    public string Status { get; set; } = string.Empty;
    public decimal AIScore { get; set; }
    public DateTime CreatedAt { get; set; }
}

public class PipelineStage
{
    public string Stage { get; set; } = string.Empty;
    public int Count { get; set; }
    public decimal TotalValue { get; set; }
}

public class LoginRequest
{
    public string Email { get; set; } = string.Empty;
    public string Password { get; set; } = string.Empty;
}

public class LoginResult
{
    public string AccessToken { get; set; } = string.Empty;
    public string RefreshToken { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;
    public string Role { get; set; } = string.Empty;
    public Guid TenantId { get; set; }
    public string FullName { get; set; } = string.Empty;
}

public class RegisterRequest
{
    public string FirstName { get; set; } = string.Empty;
    public string LastName { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;
    public string Password { get; set; } = string.Empty;
    public string TenantName { get; set; } = string.Empty;
    public string TenantSubdomain { get; set; } = string.Empty;
}

public class CreateLeadRequest
{
    public string FirstName { get; set; } = string.Empty;
    public string LastName { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;
    public string? Phone { get; set; }
    public string? Company { get; set; }
    public string? JobTitle { get; set; }
    public string Source { get; set; } = "Website";
    public string? Notes { get; set; }
    public string? Country { get; set; }
}

public class CreateContactRequest
{
    public string FirstName { get; set; } = string.Empty;
    public string LastName { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;
    public string? Phone { get; set; }
    public string? Company { get; set; }
    public string? JobTitle { get; set; }
    public string? City { get; set; }
    public string? Country { get; set; }
}

public class CreateOpportunityRequest
{
    public string Name { get; set; } = string.Empty;
    public Guid? AccountId { get; set; }
    public decimal Amount { get; set; }
    public string Currency { get; set; } = "USD";
    public decimal Probability { get; set; } = 20;
    public string Stage { get; set; } = "Prospecting";
    public DateTime? ExpectedCloseDate { get; set; }
    public string? Description { get; set; }
}

public class CreateTicketRequest
{
    public string Subject { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public string Priority { get; set; } = "Medium";
    public string? Category { get; set; }
}
