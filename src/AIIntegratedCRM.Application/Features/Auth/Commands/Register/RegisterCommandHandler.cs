using AIIntegratedCRM.Application.Common.Interfaces;
using AIIntegratedCRM.Application.Common.Models;
using AIIntegratedCRM.Domain.Entities;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace AIIntegratedCRM.Application.Features.Auth.Commands.Register;

public class RegisterCommandHandler : IRequestHandler<RegisterCommand, Result<Guid>>
{
    private readonly IApplicationDbContext _context;

    public RegisterCommandHandler(IApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<Result<Guid>> Handle(RegisterCommand request, CancellationToken cancellationToken)
    {
        var existingUser = await _context.Users
            .AnyAsync(u => u.Email == request.Email.ToLower(), cancellationToken);

        if (existingUser) return Result<Guid>.Failure("A user with this email already exists.");

        var existingTenant = await _context.Tenants
            .AnyAsync(t => t.Subdomain == request.TenantSubdomain.ToLower(), cancellationToken);

        if (existingTenant) return Result<Guid>.Failure("This subdomain is already taken.");

        var tenant = new Tenant
        {
            Name = request.TenantName,
            Subdomain = request.TenantSubdomain.ToLower(),
            AdminEmail = request.Email.ToLower(),
            IsActive = true
        };

        var user = new AppUser
        {
            TenantId = tenant.Id,
            FirstName = request.FirstName,
            LastName = request.LastName,
            Email = request.Email.ToLower(),
            PasswordHash = BCrypt.Net.BCrypt.HashPassword(request.Password),
            Role = "Admin",
            IsActive = true
        };

        await _context.Tenants.AddAsync(tenant, cancellationToken);
        await _context.Users.AddAsync(user, cancellationToken);
        await _context.SaveChangesAsync(cancellationToken);

        return Result<Guid>.Success(user.Id);
    }
}
