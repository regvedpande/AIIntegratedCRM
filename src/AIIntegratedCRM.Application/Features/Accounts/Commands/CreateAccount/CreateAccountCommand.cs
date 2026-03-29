using AIIntegratedCRM.Application.Common.Models;
using MediatR;

namespace AIIntegratedCRM.Application.Features.Accounts.Commands.CreateAccount;

public record CreateAccountCommand : IRequest<Result<Guid>>
{
    public string Name { get; init; } = string.Empty;
    public string? Industry { get; init; }
    public string? Website { get; init; }
    public string? Phone { get; init; }
    public string? Email { get; init; }
    public string? Address { get; init; }
    public string? City { get; init; }
    public string? State { get; init; }
    public string? Country { get; init; }
    public string? PostalCode { get; init; }
    public decimal? AnnualRevenue { get; init; }
    public int? EmployeeCount { get; init; }
    public string? Description { get; init; }
    public Guid? AssignedToUserId { get; init; }
    public Guid? ParentAccountId { get; init; }
    public string? Tags { get; init; }
}
