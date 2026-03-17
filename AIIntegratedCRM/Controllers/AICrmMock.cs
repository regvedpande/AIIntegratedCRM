using Microsoft.AspNetCore.Mvc;

var builder = WebApplication.CreateBuilder(args);

// Register Mock Service
builder.Services.AddSingleton<IAiCrmService, MockAiCrmService>();

var app = builder.Build();

app.MapGet("/api/aicrm/leads", async (IAiCrmService service) =>
{
    return Results.Ok(await service.GetAllLeadsAsync());
});

app.MapGet("/api/aicrm/leads/{id}", async (int id, IAiCrmService service) =>
{
    var lead = await service.GetLeadByIdAsync(id);
    return lead is not null ? Results.Ok(lead) : Results.NotFound();
});

app.MapPost("/api/aicrm/leads", async (Lead lead, IAiCrmService service) =>
{
    var created = await service.CreateLeadAsync(lead);
    return Results.Ok(created);
});

app.MapPut("/api/aicrm/leads/{id}", async (int id, Lead lead, IAiCrmService service) =>
{
    var updated = await service.UpdateLeadAsync(id, lead);
    return updated ? Results.Ok("Updated") : Results.NotFound();
});

app.MapDelete("/api/aicrm/leads/{id}", async (int id, IAiCrmService service) =>
{
    var deleted = await service.DeleteLeadAsync(id);
    return deleted ? Results.Ok("Deleted") : Results.NotFound();
});

app.Run();


// =======================
// Models
// =======================
public class Lead
{
    public int Id { get; set; }
    public string Name { get; set; }
    public string Email { get; set; }
    public string Status { get; set; }
}


// =======================
// Interface
// =======================
public interface IAiCrmService
{
    Task<List<Lead>> GetAllLeadsAsync();
    Task<Lead?> GetLeadByIdAsync(int id);
    Task<Lead> CreateLeadAsync(Lead lead);
    Task<bool> UpdateLeadAsync(int id, Lead lead);
    Task<bool> DeleteLeadAsync(int id);
}


// =======================
// Mock Implementation
// =======================
public class MockAiCrmService : IAiCrmService
{
    private readonly List<Lead> _leads = new()
    {
        new Lead { Id = 1, Name = "John Doe", Email = "john@test.com", Status = "New" },
        new Lead { Id = 2, Name = "Jane Smith", Email = "jane@test.com", Status = "Contacted" }
    };

    public Task<List<Lead>> GetAllLeadsAsync()
        => Task.FromResult(_leads);

    public Task<Lead?> GetLeadByIdAsync(int id)
        => Task.FromResult(_leads.FirstOrDefault(x => x.Id == id));

    public Task<Lead> CreateLeadAsync(Lead lead)
    {
        lead.Id = _leads.Any() ? _leads.Max(x => x.Id) + 1 : 1;
        _leads.Add(lead);
        return Task.FromResult(lead);
    }

    public Task<bool> UpdateLeadAsync(int id, Lead updatedLead)
    {
        var lead = _leads.FirstOrDefault(x => x.Id == id);
        if (lead == null) return Task.FromResult(false);

        lead.Name = updatedLead.Name;
        lead.Email = updatedLead.Email;
        lead.Status = updatedLead.Status;

        return Task.FromResult(true);
    }

    public Task<bool> DeleteLeadAsync(int id)
    {
        var lead = _leads.FirstOrDefault(x => x.Id == id);
        if (lead == null) return Task.FromResult(false);

        _leads.Remove(lead);
        return Task.FromResult(true);
    }
}