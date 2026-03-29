using AIIntegratedCRM.Application.Common.Exceptions;
using AIIntegratedCRM.Application.Common.Interfaces;
using AIIntegratedCRM.Application.Common.Models;
using AIIntegratedCRM.Domain.Entities;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace AIIntegratedCRM.Application.Features.Leads.Commands.UpdateLead;

public class UpdateLeadCommandHandler : IRequestHandler<UpdateLeadCommand, Result>
{
    private readonly IApplicationDbContext _context;
    private readonly ICurrentUserService _currentUser;

    public UpdateLeadCommandHandler(IApplicationDbContext context, ICurrentUserService currentUser)
    {
        _context = context;
        _currentUser = currentUser;
    }

    public async Task<Result> Handle(UpdateLeadCommand request, CancellationToken cancellationToken)
    {
        var lead = await _context.Leads
            .FirstOrDefaultAsync(l => l.Id == request.Id && l.TenantId == _currentUser.TenantId && !l.IsDeleted, cancellationToken);

        if (lead is null) throw new NotFoundException(nameof(Lead), request.Id);

        lead.FirstName = request.FirstName;
        lead.LastName = request.LastName;
        lead.Email = request.Email;
        lead.Phone = request.Phone;
        lead.Company = request.Company;
        lead.JobTitle = request.JobTitle;
        lead.Source = request.Source;
        lead.Status = request.Status;
        lead.Notes = request.Notes;
        lead.Tags = request.Tags;
        lead.Country = request.Country;
        lead.City = request.City;
        lead.EmployeeCount = request.EmployeeCount;
        lead.AnnualRevenue = request.AnnualRevenue;
        lead.AssignedToUserId = request.AssignedToUserId;
        lead.UpdatedAt = DateTime.UtcNow;
        lead.UpdatedBy = _currentUser.Email;

        await _context.SaveChangesAsync(cancellationToken);
        return Result.Success();
    }
}
