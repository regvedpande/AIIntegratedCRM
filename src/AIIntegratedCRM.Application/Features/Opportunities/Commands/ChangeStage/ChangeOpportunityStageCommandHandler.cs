using AIIntegratedCRM.Application.Common.Exceptions;
using AIIntegratedCRM.Application.Common.Interfaces;
using AIIntegratedCRM.Application.Common.Models;
using AIIntegratedCRM.Domain.Entities;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace AIIntegratedCRM.Application.Features.Opportunities.Commands.ChangeStage;

public class ChangeOpportunityStageCommandHandler : IRequestHandler<ChangeOpportunityStageCommand, Result>
{
    private readonly IApplicationDbContext _context;
    private readonly ICurrentUserService _currentUser;

    public ChangeOpportunityStageCommandHandler(IApplicationDbContext context, ICurrentUserService currentUser)
    {
        _context = context;
        _currentUser = currentUser;
    }

    public async Task<Result> Handle(ChangeOpportunityStageCommand request, CancellationToken cancellationToken)
    {
        var opp = await _context.Opportunities
            .FirstOrDefaultAsync(o => o.Id == request.OpportunityId && o.TenantId == _currentUser.TenantId && !o.IsDeleted, cancellationToken);

        if (opp is null) throw new NotFoundException(nameof(Opportunity), request.OpportunityId);

        opp.ChangeStage(request.NewStage, request.Reason);
        opp.UpdatedAt = DateTime.UtcNow;
        opp.UpdatedBy = _currentUser.Email;

        await _context.SaveChangesAsync(cancellationToken);
        return Result.Success();
    }
}
