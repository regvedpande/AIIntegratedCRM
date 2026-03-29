using AIIntegratedCRM.Application.Common.Interfaces;
using AIIntegratedCRM.Application.Common.Models;
using AIIntegratedCRM.Application.Features.Accounts.DTOs;
using AutoMapper;
using AutoMapper.QueryableExtensions;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace AIIntegratedCRM.Application.Features.Accounts.Queries.GetAccounts;

public class GetAccountsQueryHandler : IRequestHandler<GetAccountsQuery, Result<PaginatedList<AccountDto>>>
{
    private readonly IApplicationDbContext _context;
    private readonly ICurrentUserService _currentUser;
    private readonly IMapper _mapper;

    public GetAccountsQueryHandler(IApplicationDbContext context, ICurrentUserService currentUser, IMapper mapper)
    {
        _context = context;
        _currentUser = currentUser;
        _mapper = mapper;
    }

    public async Task<Result<PaginatedList<AccountDto>>> Handle(GetAccountsQuery request, CancellationToken cancellationToken)
    {
        var query = _context.Accounts
            .AsNoTracking()
            .Where(a => a.TenantId == _currentUser.TenantId && !a.IsDeleted);

        if (!string.IsNullOrWhiteSpace(request.SearchTerm))
        {
            var term = request.SearchTerm.ToLower();
            query = query.Where(a => a.Name.ToLower().Contains(term) ||
                (a.Industry != null && a.Industry.ToLower().Contains(term)));
        }

        if (!string.IsNullOrWhiteSpace(request.Industry))
            query = query.Where(a => a.Industry == request.Industry);

        query = request.SortBy?.ToLower() switch
        {
            "name" => request.SortDescending ? query.OrderByDescending(a => a.Name) : query.OrderBy(a => a.Name),
            "revenue" => request.SortDescending ? query.OrderByDescending(a => a.AnnualRevenue) : query.OrderBy(a => a.AnnualRevenue),
            _ => request.SortDescending ? query.OrderByDescending(a => a.CreatedAt) : query.OrderBy(a => a.CreatedAt)
        };

        var totalCount = await query.CountAsync(cancellationToken);
        var items = await query
            .Skip((request.PageNumber - 1) * request.PageSize)
            .Take(request.PageSize)
            .ProjectTo<AccountDto>(_mapper.ConfigurationProvider)
            .ToListAsync(cancellationToken);

        return Result<PaginatedList<AccountDto>>.Success(new PaginatedList<AccountDto>(items, totalCount, request.PageNumber, request.PageSize));
    }
}
