using AIIntegratedCRM.Application.Common.Exceptions;
using AIIntegratedCRM.Application.Common.Interfaces;
using AIIntegratedCRM.Application.Common.Models;
using AIIntegratedCRM.Domain.Entities;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace AIIntegratedCRM.Application.Features.Leads.Commands.ScoreLead;

public class ScoreLeadCommandHandler : IRequestHandler<ScoreLeadCommand, Result<ScoreLeadResult>>
{
    private readonly IApplicationDbContext _context;
    private readonly IAIService _aiService;
    private readonly ICurrentUserService _currentUser;

    public ScoreLeadCommandHandler(IApplicationDbContext context, IAIService aiService, ICurrentUserService currentUser)
    {
        _context = context;
        _aiService = aiService;
        _currentUser = currentUser;
    }

    public async Task<Result<ScoreLeadResult>> Handle(ScoreLeadCommand request, CancellationToken cancellationToken)
    {
        var lead = await _context.Leads
            .FirstOrDefaultAsync(l => l.Id == request.LeadId && l.TenantId == _currentUser.TenantId && !l.IsDeleted, cancellationToken);

        if (lead is null) throw new NotFoundException(nameof(Lead), request.LeadId);

        var scoreRequest = new LeadScoreRequest(
            lead.Email, lead.Company, lead.JobTitle,
            lead.Source.ToString(), lead.Country,
            lead.EmployeeCount, lead.AnnualRevenue, lead.Notes);

        var result = await _aiService.ScoreLeadAsync(scoreRequest, cancellationToken);

        lead.AIScore = result.Score;
        lead.AIScoreReason = result.Reason;
        lead.UpdatedAt = DateTime.UtcNow;
        lead.UpdatedBy = _currentUser.Email;

        await _context.SaveChangesAsync(cancellationToken);

        return Result<ScoreLeadResult>.Success(new ScoreLeadResult(result.Score, result.Reason, result.KeyFactors));
    }
}
