using AIIntegratedCRM.Application.Common.Models;
using MediatR;

namespace AIIntegratedCRM.Application.Features.Contacts.Commands.CreateContact;

public record CreateContactCommand : IRequest<Result<Guid>>
{
    public string FirstName { get; init; } = string.Empty;
    public string LastName { get; init; } = string.Empty;
    public string Email { get; init; } = string.Empty;
    public string? Phone { get; init; }
    public string? Mobile { get; init; }
    public string? Company { get; init; }
    public string? JobTitle { get; init; }
    public Guid? AccountId { get; init; }
    public string? Notes { get; init; }
    public string? Tags { get; init; }
    public string? Address { get; init; }
    public string? City { get; init; }
    public string? State { get; init; }
    public string? Country { get; init; }
    public string? PostalCode { get; init; }
    public Guid? AssignedToUserId { get; init; }
}
