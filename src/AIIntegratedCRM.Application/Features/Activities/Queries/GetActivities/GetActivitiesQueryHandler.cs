using AIIntegratedCRM.Application.Common.Interfaces;
using AIIntegratedCRM.Application.Common.Models;
using AIIntegratedCRM.Application.Features.Activities.DTOs;
using AutoMapper;
using AutoMapper.QueryableExtensions;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace AIIntegratedCRM.Application.Features.Activities.Queries.GetActivities;

public class GetActivitiesQueryHandler : IRequestHandler<GetActivitiesQuery, Result<PaginatedList<ActivityDto>>>
{
    private readonly IApplicationDbContext _context;
    private readonly ICurrentUserService _currentUser;
    private readonly IMapper _mapper;

    public GetActivitiesQueryHandler(IApplicationDbContext context, ICurrentUserService currentUser, IMapper mapper)
    {
        _context = context;
        _currentUser = currentUser;
        _mapper = mapper;
    }

    public async Task<Result<PaginatedList<ActivityDto>>> Handle(GetActivitiesQuery request, CancellationToken cancellationToken)
    {
        var query = _context.Activities
            .AsNoTracking()
            .Where(a => a.TenantId == _currentUser.TenantId && !a.IsDeleted);

        if (request.Type.HasValue) query = query.Where(a => a.Type == request.Type.Value);
        if (request.ContactId.HasValue) query = query.Where(a => a.ContactId == request.ContactId.Value);
        if (request.AccountId.HasValue) query = query.Where(a => a.AccountId == request.AccountId.Value);
        if (request.OpportunityId.HasValue) query = query.Where(a => a.OpportunityId == request.OpportunityId.Value);
        if (request.IsCompleted.HasValue) query = query.Where(a => a.IsCompleted == request.IsCompleted.Value);

        query = request.SortBy?.ToLower() switch
        {
            "subject" => request.SortDescending ? query.OrderByDescending(a => a.Subject) : query.OrderBy(a => a.Subject),
            "type" => request.SortDescending ? query.OrderByDescending(a => a.Type) : query.OrderBy(a => a.Type),
            _ => request.SortDescending ? query.OrderByDescending(a => a.StartTime) : query.OrderBy(a => a.StartTime)
        };

        var totalCount = await query.CountAsync(cancellationToken);
        var items = await query
            .Skip((request.PageNumber - 1) * request.PageSize)
            .Take(request.PageSize)
            .ProjectTo<ActivityDto>(_mapper.ConfigurationProvider)
            .ToListAsync(cancellationToken);

        return Result<PaginatedList<ActivityDto>>.Success(new PaginatedList<ActivityDto>(items, totalCount, request.PageNumber, request.PageSize));
    }
}
