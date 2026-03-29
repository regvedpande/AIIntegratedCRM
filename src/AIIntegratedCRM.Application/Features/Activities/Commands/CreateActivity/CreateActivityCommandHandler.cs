using AIIntegratedCRM.Application.Common.Interfaces;
using AIIntegratedCRM.Application.Common.Models;
using AIIntegratedCRM.Domain.Entities;
using MediatR;

namespace AIIntegratedCRM.Application.Features.Activities.Commands.CreateActivity;

public class CreateActivityCommandHandler : IRequestHandler<CreateActivityCommand, Result<Guid>>
{
    private readonly IApplicationDbContext _context;
    private readonly ICurrentUserService _currentUser;

    public CreateActivityCommandHandler(IApplicationDbContext context, ICurrentUserService currentUser)
    {
        _context = context;
        _currentUser = currentUser;
    }

    public async Task<Result<Guid>> Handle(CreateActivityCommand request, CancellationToken cancellationToken)
    {
        var activity = new Activity
        {
            Type = request.Type,
            Subject = request.Subject,
            Description = request.Description,
            StartTime = request.StartTime,
            EndTime = request.EndTime,
            DurationMinutes = request.DurationMinutes,
            ContactId = request.ContactId,
            AccountId = request.AccountId,
            OpportunityId = request.OpportunityId,
            LeadId = request.LeadId,
            AssignedToUserId = request.AssignedToUserId,
            Location = request.Location,
            MeetingUrl = request.MeetingUrl,
            Tags = request.Tags,
            TenantId = _currentUser.TenantId,
            CreatedBy = _currentUser.Email
        };

        await _context.Activities.AddAsync(activity, cancellationToken);
        await _context.SaveChangesAsync(cancellationToken);

        return Result<Guid>.Success(activity.Id);
    }
}
