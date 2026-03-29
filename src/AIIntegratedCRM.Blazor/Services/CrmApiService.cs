using AIIntegratedCRM.Blazor.Models;
using System.Net.Http.Json;
using System.Text.Json;

namespace AIIntegratedCRM.Blazor.Services;

public abstract class CrmApiServiceBase
{
    protected readonly HttpClient Http;
    protected static readonly JsonSerializerOptions JsonOptions = new() { PropertyNameCaseInsensitive = true };

    protected CrmApiServiceBase(HttpClient http) => Http = http;

    protected async Task<PaginatedResult<T>> GetPagedAsync<T>(string url)
    {
        try
        {
            var result = await Http.GetFromJsonAsync<PaginatedResult<T>>(url, JsonOptions);
            return result ?? new PaginatedResult<T>();
        }
        catch { return new PaginatedResult<T>(); }
    }

    protected async Task<T?> GetAsync<T>(string url)
    {
        try { return await Http.GetFromJsonAsync<T>(url, JsonOptions); }
        catch { return default; }
    }

    protected async Task<(bool Success, string? Error, Guid? Id)> PostAsync<TReq>(string url, TReq request)
    {
        try
        {
            var response = await Http.PostAsJsonAsync(url, request);
            if (response.IsSuccessStatusCode)
            {
                var result = await response.Content.ReadFromJsonAsync<IdResult>(JsonOptions);
                return (true, null, result?.Id);
            }
            var err = await response.Content.ReadFromJsonAsync<ErrorResponse>(JsonOptions);
            return (false, err?.Error ?? "Operation failed", null);
        }
        catch (Exception ex) { return (false, ex.Message, null); }
    }

    protected async Task<(bool Success, string? Error)> PatchAsync<TReq>(string url, TReq request)
    {
        try
        {
            var response = await Http.PatchAsJsonAsync(url, request);
            if (response.IsSuccessStatusCode) return (true, null);
            var err = await response.Content.ReadFromJsonAsync<ErrorResponse>(JsonOptions);
            return (false, err?.Error ?? "Operation failed");
        }
        catch (Exception ex) { return (false, ex.Message); }
    }

    protected async Task<(bool Success, string? Error)> DeleteAsync(string url)
    {
        try
        {
            var response = await Http.DeleteAsync(url);
            if (response.IsSuccessStatusCode) return (true, null);
            var err = await response.Content.ReadFromJsonAsync<ErrorResponse>(JsonOptions);
            return (false, err?.Error ?? "Operation failed");
        }
        catch (Exception ex) { return (false, ex.Message); }
    }
}

public record IdResult(Guid Id);

// ─── LEAD SERVICE ────────────────────────────────────────────────────────────

public interface ILeadService
{
    Task<PaginatedResult<LeadDto>> GetLeadsAsync(int page = 1, int size = 20, string? search = null, string? status = null);
    Task<(bool Success, string? Error, Guid? Id)> CreateLeadAsync(CreateLeadRequest request);
    Task<(bool Success, string? Error)> DeleteLeadAsync(Guid id);
    Task<string?> ScoreLeadAsync(Guid id);
}

public class LeadService : CrmApiServiceBase, ILeadService
{
    public LeadService(HttpClient http) : base(http) { }

    public Task<PaginatedResult<LeadDto>> GetLeadsAsync(int page = 1, int size = 20, string? search = null, string? status = null)
    {
        var url = $"api/v1/leads?pageNumber={page}&pageSize={size}";
        if (!string.IsNullOrEmpty(search)) url += $"&searchTerm={Uri.EscapeDataString(search)}";
        if (!string.IsNullOrEmpty(status)) url += $"&status={status}";
        return GetPagedAsync<LeadDto>(url);
    }

    public Task<(bool Success, string? Error, Guid? Id)> CreateLeadAsync(CreateLeadRequest request)
        => PostAsync("api/v1/leads", request);

    public Task<(bool Success, string? Error)> DeleteLeadAsync(Guid id)
        => DeleteAsync($"api/v1/leads/{id}");

    public async Task<string?> ScoreLeadAsync(Guid id)
    {
        try
        {
            var response = await Http.PostAsync($"api/v1/leads/{id}/score", null);
            if (response.IsSuccessStatusCode)
            {
                var result = await response.Content.ReadFromJsonAsync<ScoreResult>(JsonOptions);
                return result?.Reason;
            }
            return null;
        }
        catch { return null; }
    }
}

public record ScoreResult(decimal Score, string Reason, string[] KeyFactors);

// ─── CONTACT SERVICE ─────────────────────────────────────────────────────────

public interface IContactService
{
    Task<PaginatedResult<ContactDto>> GetContactsAsync(int page = 1, int size = 20, string? search = null);
    Task<(bool Success, string? Error, Guid? Id)> CreateContactAsync(CreateContactRequest request);
}

public class ContactService : CrmApiServiceBase, IContactService
{
    public ContactService(HttpClient http) : base(http) { }

    public Task<PaginatedResult<ContactDto>> GetContactsAsync(int page = 1, int size = 20, string? search = null)
    {
        var url = $"api/v1/contacts?pageNumber={page}&pageSize={size}";
        if (!string.IsNullOrEmpty(search)) url += $"&searchTerm={Uri.EscapeDataString(search)}";
        return GetPagedAsync<ContactDto>(url);
    }

    public Task<(bool Success, string? Error, Guid? Id)> CreateContactAsync(CreateContactRequest request)
        => PostAsync("api/v1/contacts", request);
}

// ─── ACCOUNT SERVICE ─────────────────────────────────────────────────────────

public interface IAccountService
{
    Task<PaginatedResult<AccountDto>> GetAccountsAsync(int page = 1, int size = 20, string? search = null);
    Task<(bool Success, string? Error, Guid? Id)> CreateAccountAsync(CreateAccountRequest request);
}

public class CreateAccountRequest
{
    public string Name { get; set; } = string.Empty;
    public string? Industry { get; set; }
    public string? Website { get; set; }
    public string? Phone { get; set; }
    public string? City { get; set; }
    public string? Country { get; set; }
    public decimal? AnnualRevenue { get; set; }
    public int? EmployeeCount { get; set; }
}

public class AccountService : CrmApiServiceBase, IAccountService
{
    public AccountService(HttpClient http) : base(http) { }

    public Task<PaginatedResult<AccountDto>> GetAccountsAsync(int page = 1, int size = 20, string? search = null)
    {
        var url = $"api/v1/accounts?pageNumber={page}&pageSize={size}";
        if (!string.IsNullOrEmpty(search)) url += $"&searchTerm={Uri.EscapeDataString(search)}";
        return GetPagedAsync<AccountDto>(url);
    }

    public Task<(bool Success, string? Error, Guid? Id)> CreateAccountAsync(CreateAccountRequest request)
        => PostAsync("api/v1/accounts", request);
}

// ─── OPPORTUNITY SERVICE ──────────────────────────────────────────────────────

public interface IOpportunityService
{
    Task<PaginatedResult<OpportunityDto>> GetOpportunitiesAsync(int page = 1, int size = 20, string? search = null, string? stage = null);
    Task<(bool Success, string? Error, Guid? Id)> CreateOpportunityAsync(CreateOpportunityRequest request);
    Task<(bool Success, string? Error)> ChangeStageAsync(Guid id, string newStage, string? reason = null);
}

public class OpportunityService : CrmApiServiceBase, IOpportunityService
{
    public OpportunityService(HttpClient http) : base(http) { }

    public Task<PaginatedResult<OpportunityDto>> GetOpportunitiesAsync(int page = 1, int size = 20, string? search = null, string? stage = null)
    {
        var url = $"api/v1/opportunities?pageNumber={page}&pageSize={size}";
        if (!string.IsNullOrEmpty(search)) url += $"&searchTerm={Uri.EscapeDataString(search)}";
        if (!string.IsNullOrEmpty(stage)) url += $"&stage={stage}";
        return GetPagedAsync<OpportunityDto>(url);
    }

    public Task<(bool Success, string? Error, Guid? Id)> CreateOpportunityAsync(CreateOpportunityRequest request)
        => PostAsync("api/v1/opportunities", request);

    public Task<(bool Success, string? Error)> ChangeStageAsync(Guid id, string newStage, string? reason = null)
        => PatchAsync($"api/v1/opportunities/{id}/stage", new { newStage, reason });
}

// ─── ACTIVITY SERVICE ────────────────────────────────────────────────────────

public interface IActivityService
{
    Task<PaginatedResult<ActivityDto>> GetActivitiesAsync(int page = 1, int size = 20);
}

public class ActivityService : CrmApiServiceBase, IActivityService
{
    public ActivityService(HttpClient http) : base(http) { }

    public Task<PaginatedResult<ActivityDto>> GetActivitiesAsync(int page = 1, int size = 20)
        => GetPagedAsync<ActivityDto>($"api/v1/activities?pageNumber={page}&pageSize={size}");
}

// ─── TICKET SERVICE ──────────────────────────────────────────────────────────

public interface ITicketService
{
    Task<PaginatedResult<SupportTicketDto>> GetTicketsAsync(int page = 1, int size = 20, string? status = null);
    Task<(bool Success, string? Error, Guid? Id)> CreateTicketAsync(CreateTicketRequest request);
}

public class TicketService : CrmApiServiceBase, ITicketService
{
    public TicketService(HttpClient http) : base(http) { }

    public Task<PaginatedResult<SupportTicketDto>> GetTicketsAsync(int page = 1, int size = 20, string? status = null)
    {
        var url = $"api/v1/supporttickets?pageNumber={page}&pageSize={size}";
        if (!string.IsNullOrEmpty(status)) url += $"&status={status}";
        return GetPagedAsync<SupportTicketDto>(url);
    }

    public Task<(bool Success, string? Error, Guid? Id)> CreateTicketAsync(CreateTicketRequest request)
        => PostAsync("api/v1/supporttickets", request);
}

// ─── AI SERVICE ──────────────────────────────────────────────────────────────

public interface IAIClientService
{
    Task<string?> AskQueryAsync(string query);
    Task<string?> GenerateEmailAsync(string purpose, string recipientName, string recipientCompany, string senderName, string tone = "Professional");
}

public class AIClientService : CrmApiServiceBase, IAIClientService
{
    public AIClientService(HttpClient http) : base(http) { }

    public async Task<string?> AskQueryAsync(string query)
    {
        try
        {
            var response = await Http.PostAsJsonAsync("api/v1/ai/query", new { query });
            if (response.IsSuccessStatusCode)
                return await response.Content.ReadAsStringAsync();
            return null;
        }
        catch { return null; }
    }

    public async Task<string?> GenerateEmailAsync(string purpose, string recipientName, string recipientCompany, string senderName, string tone = "Professional")
    {
        try
        {
            var response = await Http.PostAsJsonAsync("api/v1/ai/generate-email", new
            {
                tone, purpose, recipientName, recipientCompany, senderName, context = ""
            });
            if (response.IsSuccessStatusCode)
                return await response.Content.ReadAsStringAsync();
            return null;
        }
        catch { return null; }
    }
}

// ─── DASHBOARD SERVICE ───────────────────────────────────────────────────────

public interface IDashboardService
{
    Task<DashboardStats?> GetStatsAsync();
}

public class DashboardService : CrmApiServiceBase, IDashboardService
{
    public DashboardService(HttpClient http) : base(http) { }

    public Task<DashboardStats?> GetStatsAsync()
        => GetAsync<DashboardStats>("api/v1/dashboard/stats");
}
