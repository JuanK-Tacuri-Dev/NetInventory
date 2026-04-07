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

    private static readonly TimeSpan RefreshThreshold = Constants.Auth.RefreshThreshold;

    public AuthService(HttpClient http, IJSRuntime js)
    {
        _http = http;
        _js = js;
    }

    public async Task<string?> GetToken()
    {
        var token = await _js.InvokeAsync<string?>("localStorage.getItem", Constants.LocalStorage.TokenKey);
        if (string.IsNullOrWhiteSpace(token)) return null;

        var expiresAtStr = await _js.InvokeAsync<string?>("localStorage.getItem", Constants.LocalStorage.ExpiresAtKey);
        if (DateTime.TryParse(expiresAtStr, null, System.Globalization.DateTimeStyles.RoundtripKind, out var expiresAt))
        {
            if (DateTime.UtcNow >= expiresAt - RefreshThreshold)
            {
                var refreshed = await TryRefreshAsync();
                if (!refreshed) return null;
                return await _js.InvokeAsync<string?>("localStorage.getItem", Constants.LocalStorage.TokenKey);
            }
        }

        return token;
    }

    public async Task<bool> IsAuthenticated()
    {
        var token = await GetToken();
        return !string.IsNullOrWhiteSpace(token);
    }

    public override async Task<AuthenticationState> GetAuthenticationStateAsync()
    {
        var token = await _js.InvokeAsync<string?>("localStorage.getItem", Constants.LocalStorage.TokenKey);
        if (string.IsNullOrWhiteSpace(token))
            return Unauthenticated();

        var claims = ParseClaimsFromJwt(token);
        var identity = new ClaimsIdentity(claims, "jwt");
        return new AuthenticationState(new ClaimsPrincipal(identity));
    }

    public async Task<bool> Login(LoginRequest request)
    {
        try
        {
            var response = await _http.PostAsJsonAsync(Constants.Api.Login, request);
            if (!response.IsSuccessStatusCode) return false;

            var result = await response.Content.ReadFromJsonAsync<ApiResponse<LoginResponse>>();
            if (result is null || !result.Success || result.Data is null) return false;

            await SaveTokensAsync(result.Data);
            NotifyAuthenticationStateChanged(GetAuthenticationStateAsync());
            return true;
        }
        catch
        {
            return false;
        }
    }

    public async Task<string?> Register(RegisterRequest request)
    {
        try
        {
            var response = await _http.PostAsJsonAsync(Constants.Api.Register, request);
            if (response.IsSuccessStatusCode) return null;

            var result = await response.Content.ReadFromJsonAsync<ApiResponse>();
            return result?.Error ?? "No se pudo crear la cuenta. Intenta de nuevo.";
        }
        catch
        {
            return "Error al conectar con el servidor.";
        }
    }

    public async Task Logout()
    {
        await _js.InvokeVoidAsync("localStorage.removeItem", Constants.LocalStorage.TokenKey);
        await _js.InvokeVoidAsync("localStorage.removeItem", Constants.LocalStorage.ExpiresAtKey);
        await _js.InvokeVoidAsync("localStorage.removeItem", Constants.LocalStorage.RefreshTokenKey);
        NotifyAuthenticationStateChanged(Task.FromResult(Unauthenticated()));
    }

    public async Task<bool> TryRefreshAsync()
    {
        try
        {
            var refreshToken = await _js.InvokeAsync<string?>("localStorage.getItem", Constants.LocalStorage.RefreshTokenKey);
            if (string.IsNullOrWhiteSpace(refreshToken)) return false;

            var response = await _http.PostAsJsonAsync(Constants.Api.Refresh, new { refreshToken });
            if (!response.IsSuccessStatusCode) { await Logout(); return false; }

            var result = await response.Content.ReadFromJsonAsync<ApiResponse<LoginResponse>>();
            if (result is null || !result.Success || result.Data is null) { await Logout(); return false; }

            await SaveTokensAsync(result.Data);
            NotifyAuthenticationStateChanged(GetAuthenticationStateAsync());
            return true;
        }
        catch
        {
            return false;
        }
    }

    private async Task SaveTokensAsync(LoginResponse data)
    {
        await _js.InvokeVoidAsync("localStorage.setItem", Constants.LocalStorage.TokenKey, data.Token);
        await _js.InvokeVoidAsync("localStorage.setItem", Constants.LocalStorage.ExpiresAtKey, data.ExpiresAt.ToString("O"));
        if (!string.IsNullOrEmpty(data.RefreshToken))
            await _js.InvokeVoidAsync("localStorage.setItem", Constants.LocalStorage.RefreshTokenKey, data.RefreshToken);
    }

    private static AuthenticationState Unauthenticated()
        => new(new ClaimsPrincipal(new ClaimsIdentity()));

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

    private record LoginResponse(
        string Token,
        DateTime ExpiresAt,
        string? RefreshToken,
        DateTime? RefreshTokenExpiresAt);
}
