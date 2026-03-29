using AIIntegratedCRM.Application.Common.Models;
using AIIntegratedCRM.Application.Features.Leads.DTOs;
using AIIntegratedCRM.Domain.Enums;
using MediatR;

namespace AIIntegratedCRM.Application.Features.Leads.Queries.GetLeads;

public record GetLeadsQuery : IRequest<Result<PaginatedList<LeadDto>>>
{
    public int PageNumber { get; init; } = 1;
    public int PageSize { get; init; } = 20;
    public string? SearchTerm { get; init; }
    public LeadStatus? Status { get; init; }
    public LeadSource? Source { get; init; }
    public Guid? AssignedToUserId { get; init; }
    public string? SortBy { get; init; } = "CreatedAt";
    public bool SortDescending { get; init; } = true;
}
