using AIIntegratedCRM.Application.Common.Interfaces;
using AIIntegratedCRM.Application.Common.Models;
using AIIntegratedCRM.Application.Features.SupportTickets.DTOs;
using AutoMapper;
using AutoMapper.QueryableExtensions;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace AIIntegratedCRM.Application.Features.SupportTickets.Queries.GetTickets;

public class GetTicketsQueryHandler : IRequestHandler<GetTicketsQuery, Result<PaginatedList<SupportTicketDto>>>
{
    private readonly IApplicationDbContext _context;
    private readonly ICurrentUserService _currentUser;
    private readonly IMapper _mapper;

    public GetTicketsQueryHandler(IApplicationDbContext context, ICurrentUserService currentUser, IMapper mapper)
    {
        _context = context;
        _currentUser = currentUser;
        _mapper = mapper;
    }

    public async Task<Result<PaginatedList<SupportTicketDto>>> Handle(GetTicketsQuery request, CancellationToken cancellationToken)
    {
        var query = _context.SupportTickets
            .AsNoTracking()
            .Include(t => t.Comments)
            .Where(t => t.TenantId == _currentUser.TenantId && !t.IsDeleted);

        if (!string.IsNullOrWhiteSpace(request.SearchTerm))
        {
            var term = request.SearchTerm.ToLower();
            query = query.Where(t => t.Subject.ToLower().Contains(term) || t.Description.ToLower().Contains(term));
        }

        if (request.Status.HasValue) query = query.Where(t => t.Status == request.Status.Value);
        if (request.Priority.HasValue) query = query.Where(t => t.Priority == request.Priority.Value);
        if (request.AssignedToUserId.HasValue) query = query.Where(t => t.AssignedToUserId == request.AssignedToUserId.Value);

        query = request.SortBy?.ToLower() switch
        {
            "subject" => request.SortDescending ? query.OrderByDescending(t => t.Subject) : query.OrderBy(t => t.Subject),
            "priority" => request.SortDescending ? query.OrderByDescending(t => t.Priority) : query.OrderBy(t => t.Priority),
            "status" => request.SortDescending ? query.OrderByDescending(t => t.Status) : query.OrderBy(t => t.Status),
            _ => request.SortDescending ? query.OrderByDescending(t => t.CreatedAt) : query.OrderBy(t => t.CreatedAt)
        };

        var totalCount = await query.CountAsync(cancellationToken);
        var items = await query
            .Skip((request.PageNumber - 1) * request.PageSize)
            .Take(request.PageSize)
            .ProjectTo<SupportTicketDto>(_mapper.ConfigurationProvider)
            .ToListAsync(cancellationToken);

        return Result<PaginatedList<SupportTicketDto>>.Success(new PaginatedList<SupportTicketDto>(items, totalCount, request.PageNumber, request.PageSize));
    }
}
