using AIIntegratedCRM.Application.Common.Models;
using AIIntegratedCRM.Application.Features.Activities.DTOs;
using AIIntegratedCRM.Domain.Enums;
using MediatR;

namespace AIIntegratedCRM.Application.Features.Activities.Queries.GetActivities;

public record GetActivitiesQuery : IRequest<Result<PaginatedList<ActivityDto>>>
{
    public int PageNumber { get; init; } = 1;
    public int PageSize { get; init; } = 20;
    public ActivityType? Type { get; init; }
    public Guid? ContactId { get; init; }
    public Guid? AccountId { get; init; }
    public Guid? OpportunityId { get; init; }
    public bool? IsCompleted { get; init; }
    public string? SortBy { get; init; } = "StartTime";
    public bool SortDescending { get; init; } = true;
}
