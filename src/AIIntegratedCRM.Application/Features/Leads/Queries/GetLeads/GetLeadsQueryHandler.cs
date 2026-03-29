using AIIntegratedCRM.Application.Common.Interfaces;
using AIIntegratedCRM.Application.Common.Models;
using AIIntegratedCRM.Application.Features.Leads.DTOs;
using AutoMapper;
using AutoMapper.QueryableExtensions;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace AIIntegratedCRM.Application.Features.Leads.Queries.GetLeads;

public class GetLeadsQueryHandler : IRequestHandler<GetLeadsQuery, Result<PaginatedList<LeadDto>>>
{
    private readonly IApplicationDbContext _context;
    private readonly ICurrentUserService _currentUser;
    private readonly IMapper _mapper;

    public GetLeadsQueryHandler(IApplicationDbContext context, ICurrentUserService currentUser, IMapper mapper)
    {
        _context = context;
        _currentUser = currentUser;
        _mapper = mapper;
    }

    public async Task<Result<PaginatedList<LeadDto>>> Handle(GetLeadsQuery request, CancellationToken cancellationToken)
    {
        var query = _context.Leads
            .AsNoTracking()
            .Where(l => l.TenantId == _currentUser.TenantId && !l.IsDeleted);

        if (!string.IsNullOrWhiteSpace(request.SearchTerm))
        {
            var term = request.SearchTerm.ToLower();
            query = query.Where(l =>
                l.FirstName.ToLower().Contains(term) ||
                l.LastName.ToLower().Contains(term) ||
                l.Email.ToLower().Contains(term) ||
                (l.Company != null && l.Company.ToLower().Contains(term)));
        }

        if (request.Status.HasValue) query = query.Where(l => l.Status == request.Status.Value);
        if (request.Source.HasValue) query = query.Where(l => l.Source == request.Source.Value);
        if (request.AssignedToUserId.HasValue) query = query.Where(l => l.AssignedToUserId == request.AssignedToUserId.Value);

        query = request.SortBy?.ToLower() switch
        {
            "name" => request.SortDescending ? query.OrderByDescending(l => l.FirstName) : query.OrderBy(l => l.FirstName),
            "email" => request.SortDescending ? query.OrderByDescending(l => l.Email) : query.OrderBy(l => l.Email),
            "score" => request.SortDescending ? query.OrderByDescending(l => l.AIScore) : query.OrderBy(l => l.AIScore),
            "status" => request.SortDescending ? query.OrderByDescending(l => l.Status) : query.OrderBy(l => l.Status),
            _ => request.SortDescending ? query.OrderByDescending(l => l.CreatedAt) : query.OrderBy(l => l.CreatedAt)
        };

        var totalCount = await query.CountAsync(cancellationToken);
        var items = await query
            .Skip((request.PageNumber - 1) * request.PageSize)
            .Take(request.PageSize)
            .ProjectTo<LeadDto>(_mapper.ConfigurationProvider)
            .ToListAsync(cancellationToken);

        return Result<PaginatedList<LeadDto>>.Success(new PaginatedList<LeadDto>(items, totalCount, request.PageNumber, request.PageSize));
    }
}
