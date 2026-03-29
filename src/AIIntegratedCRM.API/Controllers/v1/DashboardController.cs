using AIIntegratedCRM.Application.Common.Interfaces;
using AIIntegratedCRM.Domain.Enums;
using Asp.Versioning;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace AIIntegratedCRM.API.Controllers.v1;

[ApiVersion("1.0")]
[Authorize]
public class DashboardController : ApiControllerBase
{
    private readonly IApplicationDbContext _context;
    private readonly ICurrentUserService _currentUser;

    public DashboardController(IApplicationDbContext context, ICurrentUserService currentUser)
    {
        _context = context;
        _currentUser = currentUser;
    }

    [HttpGet("stats")]
    public async Task<IActionResult> GetStats(CancellationToken ct)
    {
        var tenantId = _currentUser.TenantId;

        var totalLeads = await _context.Leads.CountAsync(l => l.TenantId == tenantId, ct);
        var newLeads = await _context.Leads.CountAsync(l => l.TenantId == tenantId && l.Status == LeadStatus.New, ct);
        var totalContacts = await _context.Contacts.CountAsync(c => c.TenantId == tenantId, ct);
        var totalAccounts = await _context.Accounts.CountAsync(a => a.TenantId == tenantId, ct);
        var totalOpportunities = await _context.Opportunities.CountAsync(o => o.TenantId == tenantId, ct);
        var openOpportunities = await _context.Opportunities.CountAsync(o => o.TenantId == tenantId && o.Stage != OpportunityStage.ClosedWon && o.Stage != OpportunityStage.ClosedLost, ct);
        var totalPipelineValue = await _context.Opportunities.Where(o => o.TenantId == tenantId && o.Stage != OpportunityStage.ClosedLost).SumAsync(o => o.Amount, ct);
        var wonOpportunities = await _context.Opportunities.CountAsync(o => o.TenantId == tenantId && o.Stage == OpportunityStage.ClosedWon, ct);
        var totalTickets = await _context.SupportTickets.CountAsync(t => t.TenantId == tenantId, ct);
        var openTickets = await _context.SupportTickets.CountAsync(t => t.TenantId == tenantId && t.Status == TicketStatus.Open, ct);

        var recentLeads = await _context.Leads
            .Where(l => l.TenantId == tenantId)
            .OrderByDescending(l => l.CreatedAt)
            .Take(5)
            .Select(l => new { l.Id, l.FirstName, l.LastName, l.Email, l.Company, l.Status, l.AIScore, l.CreatedAt })
            .ToListAsync(ct);

        var pipelineByStage = await _context.Opportunities
            .Where(o => o.TenantId == tenantId)
            .GroupBy(o => o.Stage)
            .Select(g => new { Stage = g.Key, Count = g.Count(), TotalValue = g.Sum(o => o.Amount) })
            .ToListAsync(ct);

        return Ok(new
        {
            totalLeads, newLeads, totalContacts, totalAccounts,
            totalOpportunities, openOpportunities, totalPipelineValue,
            wonOpportunities, totalTickets, openTickets,
            recentLeads, pipelineByStage
        });
    }
}
