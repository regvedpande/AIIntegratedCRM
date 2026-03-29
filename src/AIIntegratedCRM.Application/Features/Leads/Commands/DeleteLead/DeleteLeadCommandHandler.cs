using AIIntegratedCRM.Application.Common.Exceptions;
using AIIntegratedCRM.Application.Common.Interfaces;
using AIIntegratedCRM.Application.Common.Models;
using AIIntegratedCRM.Domain.Entities;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace AIIntegratedCRM.Application.Features.Leads.Commands.DeleteLead;

public class DeleteLeadCommandHandler : IRequestHandler<DeleteLeadCommand, Result>
{
    private readonly IApplicationDbContext _context;
    private readonly ICurrentUserService _currentUser;

    public DeleteLeadCommandHandler(IApplicationDbContext context, ICurrentUserService currentUser)
    {
        _context = context;
        _currentUser = currentUser;
    }

    public async Task<Result> Handle(DeleteLeadCommand request, CancellationToken cancellationToken)
    {
        var lead = await _context.Leads
            .FirstOrDefaultAsync(l => l.Id == request.Id && l.TenantId == _currentUser.TenantId && !l.IsDeleted, cancellationToken);

        if (lead is null) throw new NotFoundException(nameof(Lead), request.Id);

        lead.IsDeleted = true;
        lead.DeletedAt = DateTime.UtcNow;
        lead.DeletedBy = _currentUser.Email;

        await _context.SaveChangesAsync(cancellationToken);
        return Result.Success();
    }
}
