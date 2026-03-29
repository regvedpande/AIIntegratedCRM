using AIIntegratedCRM.Application.Common.Models;
using AIIntegratedCRM.Application.Features.Opportunities.DTOs;
using AIIntegratedCRM.Domain.Enums;
using MediatR;

namespace AIIntegratedCRM.Application.Features.Opportunities.Queries.GetOpportunities;

public record GetOpportunitiesQuery : IRequest<Result<PaginatedList<OpportunityDto>>>
{
    public int PageNumber { get; init; } = 1;
    public int PageSize { get; init; } = 20;
    public string? SearchTerm { get; init; }
    public OpportunityStage? Stage { get; init; }
    public Guid? AccountId { get; init; }
    public Guid? AssignedToUserId { get; init; }
    public string? SortBy { get; init; } = "CreatedAt";
    public bool SortDescending { get; init; } = true;
}
