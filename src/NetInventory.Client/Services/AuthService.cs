using System.Net.Http.Json;
using System.Security.Claims;
using System.Text.Json;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.JSInterop;
using NetInventory.Client.Models;

namespace NetInventory.Client.Services;

public class AuthService : AuthenticationStateProvider
{
    private readonly HttpClient _http;
    private readonly IJSRuntime _js;
    private const string TokenKey = "auth_token";

    public AuthService(HttpClient http, IJSRuntime js)
    {
        _http = http;
        _js = js;
    }

    public async Task<string?> GetToken()
    {
        return await _js.InvokeAsync<string?>("localStorage.getItem", TokenKey);
    }

    public async Task<bool> IsAuthenticated()
    {
        var token = await GetToken();
        return !string.IsNullOrWhiteSpace(token);
    }

    public override async Task<AuthenticationState> GetAuthenticationStateAsync()
    {
        var token = await GetToken();
        if (string.IsNullOrWhiteSpace(token))
            return new AuthenticationState(new ClaimsPrincipal(new ClaimsIdentity()));

        var claims = ParseClaimsFromJwt(token);
        var identity = new ClaimsIdentity(claims, "jwt");
        return new AuthenticationState(new ClaimsPrincipal(identity));
    }

    public async Task<bool> Login(LoginRequest request)
    {
        try
        {
            var response = await _http.PostAsJsonAsync("/api/auth/login", request);
            if (!response.IsSuccessStatusCode) return false;

            var result = await response.Content.ReadFromJsonAsync<ApiResponse<LoginResponse>>();
            if (result is null || !result.Success || result.Data is null) return false;

            await _js.InvokeVoidAsync("localStorage.setItem", TokenKey, result.Data.Token);
            NotifyAuthenticationStateChanged(GetAuthenticationStateAsync());
            return true;
        }
        catch
        {
            return false;
        }
    }

    public async Task<bool> Register(RegisterRequest request)
    {
        try
        {
            var response = await _http.PostAsJsonAsync("/api/auth/register", request);
            return response.IsSuccessStatusCode;
        }
        catch
        {
            return false;
        }
    }

    public async Task Logout()
    {
        await _js.InvokeVoidAsync("localStorage.removeItem", TokenKey);
        NotifyAuthenticationStateChanged(GetAuthenticationStateAsync());
    }

    private static IEnumerable<Claim> ParseClaimsFromJwt(string jwt)
    {
        var claims = new List<Claim>();
        var payload = jwt.Split('.')[1];
        var jsonBytes = ParseBase64WithoutPadding(payload);
        var kvs = JsonSerializer.Deserialize<Dictionary<string, JsonElement>>(jsonBytes);
        if (kvs is null) return claims;

        foreach (var kv in kvs)
        {
            if (kv.Value.ValueKind == JsonValueKind.Array)
            {
                foreach (var item in kv.Value.EnumerateArray())
                    claims.Add(new Claim(kv.Key, item.GetString() ?? string.Empty));
            }
            else
            {
                claims.Add(new Claim(kv.Key, kv.Value.ToString()));
            }
        }

        return claims;
    }

    private static byte[] ParseBase64WithoutPadding(string base64)
    {
        base64 = base64.Replace('-', '+').Replace('_', '/');
        base64 = base64.PadRight(base64.Length + (4 - base64.Length % 4) % 4, '=');
        return Convert.FromBase64String(base64);
    }

    private record LoginResponse(string Token, DateTime ExpiresAt);
}
