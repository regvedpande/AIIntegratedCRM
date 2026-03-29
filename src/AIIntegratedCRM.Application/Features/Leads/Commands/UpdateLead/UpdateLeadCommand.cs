using AIIntegratedCRM.Application.Common.Models;
using AIIntegratedCRM.Domain.Enums;
using MediatR;

namespace AIIntegratedCRM.Application.Features.Leads.Commands.UpdateLead;

public record UpdateLeadCommand : IRequest<Result>
{
    public Guid Id { get; init; }
    public string FirstName { get; init; } = string.Empty;
    public string LastName { get; init; } = string.Empty;
    public string Email { get; init; } = string.Empty;
    public string? Phone { get; init; }
    public string? Company { get; init; }
    public string? JobTitle { get; init; }
    public LeadSource Source { get; init; }
    public LeadStatus Status { get; init; }
    public string? Notes { get; init; }
    public string? Tags { get; init; }
    public string? Country { get; init; }
    public string? City { get; init; }
    public int? EmployeeCount { get; init; }
    public int? AnnualRevenue { get; init; }
    public Guid? AssignedToUserId { get; init; }
}
