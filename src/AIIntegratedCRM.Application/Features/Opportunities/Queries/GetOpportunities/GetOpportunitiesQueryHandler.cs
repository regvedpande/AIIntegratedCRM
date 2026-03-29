using AIIntegratedCRM.Application.Common.Interfaces;
using AIIntegratedCRM.Application.Common.Models;
using AIIntegratedCRM.Application.Features.Opportunities.DTOs;
using AutoMapper;
using AutoMapper.QueryableExtensions;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace AIIntegratedCRM.Application.Features.Opportunities.Queries.GetOpportunities;

public class GetOpportunitiesQueryHandler : IRequestHandler<GetOpportunitiesQuery, Result<PaginatedList<OpportunityDto>>>
{
    private readonly IApplicationDbContext _context;
    private readonly ICurrentUserService _currentUser;
    private readonly IMapper _mapper;

    public GetOpportunitiesQueryHandler(IApplicationDbContext context, ICurrentUserService currentUser, IMapper mapper)
    {
        _context = context;
        _currentUser = currentUser;
        _mapper = mapper;
    }

    public async Task<Result<PaginatedList<OpportunityDto>>> Handle(GetOpportunitiesQuery request, CancellationToken cancellationToken)
    {
        var query = _context.Opportunities
            .AsNoTracking()
            .Where(o => o.TenantId == _currentUser.TenantId && !o.IsDeleted);

        if (!string.IsNullOrWhiteSpace(request.SearchTerm))
        {
            var term = request.SearchTerm.ToLower();
            query = query.Where(o => o.Name.ToLower().Contains(term));
        }

        if (request.Stage.HasValue) query = query.Where(o => o.Stage == request.Stage.Value);
        if (request.AccountId.HasValue) query = query.Where(o => o.AccountId == request.AccountId.Value);
        if (request.AssignedToUserId.HasValue) query = query.Where(o => o.AssignedToUserId == request.AssignedToUserId.Value);

        query = request.SortBy?.ToLower() switch
        {
            "name" => request.SortDescending ? query.OrderByDescending(o => o.Name) : query.OrderBy(o => o.Name),
            "amount" => request.SortDescending ? query.OrderByDescending(o => o.Amount) : query.OrderBy(o => o.Amount),
            "probability" => request.SortDescending ? query.OrderByDescending(o => o.AIProbability) : query.OrderBy(o => o.AIProbability),
            "closedate" => request.SortDescending ? query.OrderByDescending(o => o.ExpectedCloseDate) : query.OrderBy(o => o.ExpectedCloseDate),
            _ => request.SortDescending ? query.OrderByDescending(o => o.CreatedAt) : query.OrderBy(o => o.CreatedAt)
        };

        var totalCount = await query.CountAsync(cancellationToken);
        var items = await query
            .Skip((request.PageNumber - 1) * request.PageSize)
            .Take(request.PageSize)
            .ProjectTo<OpportunityDto>(_mapper.ConfigurationProvider)
            .ToListAsync(cancellationToken);

        return Result<PaginatedList<OpportunityDto>>.Success(new PaginatedList<OpportunityDto>(items, totalCount, request.PageNumber, request.PageSize));
    }
}
