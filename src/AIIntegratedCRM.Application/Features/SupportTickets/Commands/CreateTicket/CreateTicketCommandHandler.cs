using AIIntegratedCRM.Application.Common.Interfaces;
using AIIntegratedCRM.Application.Common.Models;
using AIIntegratedCRM.Domain.Entities;
using MediatR;

namespace AIIntegratedCRM.Application.Features.SupportTickets.Commands.CreateTicket;

public class CreateTicketCommandHandler : IRequestHandler<CreateTicketCommand, Result<Guid>>
{
    private readonly IApplicationDbContext _context;
    private readonly ICurrentUserService _currentUser;

    public CreateTicketCommandHandler(IApplicationDbContext context, ICurrentUserService currentUser)
    {
        _context = context;
        _currentUser = currentUser;
    }

    public async Task<Result<Guid>> Handle(CreateTicketCommand request, CancellationToken cancellationToken)
    {
        var ticket = new SupportTicket
        {
            Subject = request.Subject,
            Description = request.Description,
            Priority = request.Priority,
            ContactId = request.ContactId,
            AccountId = request.AccountId,
            AssignedToUserId = request.AssignedToUserId,
            Tags = request.Tags,
            Category = request.Category,
            DueDate = request.DueDate,
            TenantId = _currentUser.TenantId,
            CreatedBy = _currentUser.Email
        };

        await _context.SupportTickets.AddAsync(ticket, cancellationToken);
        await _context.SaveChangesAsync(cancellationToken);

        return Result<Guid>.Success(ticket.Id);
    }
}
