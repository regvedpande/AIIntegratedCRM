using AIIntegratedCRM.Blazor.Models;
using Blazored.LocalStorage;
using Microsoft.AspNetCore.Components.Authorization;
using System.Net.Http.Json;

namespace AIIntegratedCRM.Blazor.Services;

public interface IAuthService
{
    Task<(bool Success, string? Error)> LoginAsync(LoginRequest request);
    Task<(bool Success, string? Error)> RegisterAsync(RegisterRequest request);
    Task LogoutAsync();
    Task<bool> IsAuthenticatedAsync();
}

public class AuthService : IAuthService
{
    private readonly HttpClient _http;
    private readonly ILocalStorageService _localStorage;
    private readonly JwtAuthStateProvider _authStateProvider;

    public AuthService(HttpClient http, ILocalStorageService localStorage, AuthenticationStateProvider authStateProvider)
    {
        _http = http;
        _localStorage = localStorage;
        _authStateProvider = (JwtAuthStateProvider)authStateProvider;
    }

    public async Task<(bool Success, string? Error)> LoginAsync(LoginRequest request)
    {
        try
        {
            var response = await _http.PostAsJsonAsync("api/v1/auth/login", request);
            if (!response.IsSuccessStatusCode)
            {
                var err = await response.Content.ReadFromJsonAsync<ErrorResponse>();
                return (false, err?.Error ?? "Login failed");
            }
            var result = await response.Content.ReadFromJsonAsync<LoginResult>();
            if (result is null) return (false, "Invalid response from server");

            await _localStorage.SetItemAsStringAsync("access_token", result.AccessToken);
            await _localStorage.SetItemAsStringAsync("refresh_token", result.RefreshToken);
            await _localStorage.SetItemAsStringAsync("user_email", result.Email);
            await _localStorage.SetItemAsStringAsync("user_name", result.FullName);
            await _localStorage.SetItemAsStringAsync("user_role", result.Role);

            _authStateProvider.NotifyAuthStateChanged();
            return (true, null);
        }
        catch (Exception ex)
        {
            return (false, ex.Message);
        }
    }

    public async Task<(bool Success, string? Error)> RegisterAsync(RegisterRequest request)
    {
        try
        {
            var response = await _http.PostAsJsonAsync("api/v1/auth/register", request);
            if (!response.IsSuccessStatusCode)
            {
                var err = await response.Content.ReadFromJsonAsync<ErrorResponse>();
                return (false, err?.Error ?? "Registration failed");
            }
            return (true, null);
        }
        catch (Exception ex)
        {
            return (false, ex.Message);
        }
    }

    public async Task LogoutAsync()
    {
        await _localStorage.RemoveItemAsync("access_token");
        await _localStorage.RemoveItemAsync("refresh_token");
        await _localStorage.RemoveItemAsync("user_email");
        await _localStorage.RemoveItemAsync("user_name");
        await _localStorage.RemoveItemAsync("user_role");
        _authStateProvider.NotifyAuthStateChanged();
    }

    public async Task<bool> IsAuthenticatedAsync()
    {
        var token = await _localStorage.GetItemAsStringAsync("access_token");
        return !string.IsNullOrWhiteSpace(token);
    }
}

public record ErrorResponse(string? Error);
