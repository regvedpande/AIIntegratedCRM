using AIIntegratedCRM.Application.Common.Interfaces;
using AIIntegratedCRM.Application.Common.Models;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace AIIntegratedCRM.Application.Features.AI.Queries.NaturalLanguageQuery;

public class NLQueryCommandHandler : IRequestHandler<NLQueryCommand, Result<string>>
{
    private readonly IApplicationDbContext _context;
    private readonly IAIService _aiService;
    private readonly ICurrentUserService _currentUser;

    public NLQueryCommandHandler(IApplicationDbContext context, IAIService aiService, ICurrentUserService currentUser)
    {
        _context = context;
        _aiService = aiService;
        _currentUser = currentUser;
    }

    public async Task<Result<string>> Handle(NLQueryCommand request, CancellationToken cancellationToken)
    {
        var leadCount = await _context.Leads.CountAsync(l => l.TenantId == _currentUser.TenantId && !l.IsDeleted, cancellationToken);
        var contactCount = await _context.Contacts.CountAsync(c => c.TenantId == _currentUser.TenantId && !c.IsDeleted, cancellationToken);
        var oppCount = await _context.Opportunities.CountAsync(o => o.TenantId == _currentUser.TenantId && !o.IsDeleted, cancellationToken);
        var ticketCount = await _context.SupportTickets.CountAsync(t => t.TenantId == _currentUser.TenantId && !t.IsDeleted, cancellationToken);
        var totalRevenue = await _context.Opportunities
            .Where(o => o.TenantId == _currentUser.TenantId && !o.IsDeleted)
            .SumAsync(o => o.Amount, cancellationToken);

        var context = $"CRM Data Summary: Leads={leadCount}, Contacts={contactCount}, Opportunities={oppCount}, SupportTickets={ticketCount}, TotalPipelineRevenue={totalRevenue:C}";
        var answer = await _aiService.AnswerNaturalLanguageQueryAsync(request.Query, context, cancellationToken);

        return Result<string>.Success(answer);
    }
}
