using AIIntegratedCRM.Application.Common.Interfaces;
using AIIntegratedCRM.Application.Common.Models;
using AIIntegratedCRM.Domain.Entities;
using MediatR;

namespace AIIntegratedCRM.Application.Features.Leads.Commands.CreateLead;

public class CreateLeadCommandHandler : IRequestHandler<CreateLeadCommand, Result<Guid>>
{
    private readonly IApplicationDbContext _context;
    private readonly ICurrentUserService _currentUser;

    public CreateLeadCommandHandler(IApplicationDbContext context, ICurrentUserService currentUser)
    {
        _context = context;
        _currentUser = currentUser;
    }

    public async Task<Result<Guid>> Handle(CreateLeadCommand request, CancellationToken cancellationToken)
    {
        var lead = Lead.Create(request.FirstName, request.LastName, request.Email, _currentUser.TenantId, _currentUser.Email);
        lead.Phone = request.Phone;
        lead.Company = request.Company;
        lead.JobTitle = request.JobTitle;
        lead.Source = request.Source;
        lead.Notes = request.Notes;
        lead.Tags = request.Tags;
        lead.Website = request.Website;
        lead.Country = request.Country;
        lead.City = request.City;
        lead.EmployeeCount = request.EmployeeCount;
        lead.AnnualRevenue = request.AnnualRevenue;
        lead.AssignedToUserId = request.AssignedToUserId;

        await _context.Leads.AddAsync(lead, cancellationToken);
        await _context.SaveChangesAsync(cancellationToken);

        return Result<Guid>.Success(lead.Id);
    }
}
