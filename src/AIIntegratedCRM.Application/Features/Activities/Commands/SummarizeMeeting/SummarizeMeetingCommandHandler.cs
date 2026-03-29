using AIIntegratedCRM.Application.Common.Exceptions;
using AIIntegratedCRM.Application.Common.Interfaces;
using AIIntegratedCRM.Application.Common.Models;
using AIIntegratedCRM.Domain.Entities;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace AIIntegratedCRM.Application.Features.Activities.Commands.SummarizeMeeting;

public class SummarizeMeetingCommandHandler : IRequestHandler<SummarizeMeetingCommand, Result<string>>
{
    private readonly IApplicationDbContext _context;
    private readonly IAIService _aiService;
    private readonly ICurrentUserService _currentUser;

    public SummarizeMeetingCommandHandler(IApplicationDbContext context, IAIService aiService, ICurrentUserService currentUser)
    {
        _context = context;
        _aiService = aiService;
        _currentUser = currentUser;
    }

    public async Task<Result<string>> Handle(SummarizeMeetingCommand request, CancellationToken cancellationToken)
    {
        var activity = await _context.Activities
            .FirstOrDefaultAsync(a => a.Id == request.ActivityId && a.TenantId == _currentUser.TenantId && !a.IsDeleted, cancellationToken);

        if (activity is null) throw new NotFoundException(nameof(Activity), request.ActivityId);

        var summary = await _aiService.SummarizeMeetingAsync(request.Transcript, cancellationToken);
        activity.AISummary = summary;
        activity.TranscriptText = request.Transcript;
        activity.UpdatedAt = DateTime.UtcNow;
        activity.UpdatedBy = _currentUser.Email;

        await _context.SaveChangesAsync(cancellationToken);
        return Result<string>.Success(summary);
    }
}
