using AIIntegratedCRM.Application.Common.Models;
using AIIntegratedCRM.Domain.Enums;
using MediatR;

namespace AIIntegratedCRM.Application.Features.Activities.Commands.CreateActivity;

public record CreateActivityCommand : IRequest<Result<Guid>>
{
    public ActivityType Type { get; init; }
    public string Subject { get; init; } = string.Empty;
    public string? Description { get; init; }
    public DateTime StartTime { get; init; }
    public DateTime? EndTime { get; init; }
    public int? DurationMinutes { get; init; }
    public Guid? ContactId { get; init; }
    public Guid? AccountId { get; init; }
    public Guid? OpportunityId { get; init; }
    public Guid? LeadId { get; init; }
    public Guid? AssignedToUserId { get; init; }
    public string? Location { get; init; }
    public string? MeetingUrl { get; init; }
    public string? Tags { get; init; }
}
