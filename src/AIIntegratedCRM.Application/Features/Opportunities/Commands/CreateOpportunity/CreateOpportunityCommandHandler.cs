using AIIntegratedCRM.Application.Common.Interfaces;
using AIIntegratedCRM.Application.Common.Models;
using AIIntegratedCRM.Domain.Entities;
using MediatR;

namespace AIIntegratedCRM.Application.Features.Opportunities.Commands.CreateOpportunity;

public class CreateOpportunityCommandHandler : IRequestHandler<CreateOpportunityCommand, Result<Guid>>
{
    private readonly IApplicationDbContext _context;
    private readonly ICurrentUserService _currentUser;

    public CreateOpportunityCommandHandler(IApplicationDbContext context, ICurrentUserService currentUser)
    {
        _context = context;
        _currentUser = currentUser;
    }

    public async Task<Result<Guid>> Handle(CreateOpportunityCommand request, CancellationToken cancellationToken)
    {
        var opportunity = new Opportunity
        {
            Name = request.Name,
            AccountId = request.AccountId,
            PrimaryContactId = request.PrimaryContactId,
            AssignedToUserId = request.AssignedToUserId,
            Stage = request.Stage,
            Amount = request.Amount,
            Currency = request.Currency,
            Probability = request.Probability,
            AIProbability = request.Probability,
            ExpectedCloseDate = request.ExpectedCloseDate,
            Description = request.Description,
            NextStep = request.NextStep,
            Tags = request.Tags,
            LeadSource = request.LeadSource,
            TenantId = _currentUser.TenantId,
            CreatedBy = _currentUser.Email
        };

        await _context.Opportunities.AddAsync(opportunity, cancellationToken);
        await _context.SaveChangesAsync(cancellationToken);

        return Result<Guid>.Success(opportunity.Id);
    }
}
