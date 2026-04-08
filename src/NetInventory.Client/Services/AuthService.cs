using System.Security.Claims;
using System.Text.Json;
using Microsoft.AspNetCore.Components.Authorization;
using NetInventory.Client.Models;

namespace NetInventory.Client.Services;

public class AuthService(
    ApiClientService api,
    TokenStoreService tokenStore) : AuthenticationStateProvider
{
    public async Task<string?> GetToken() => await tokenStore.GetTokenAsync();

    public async Task<bool> IsAuthenticated()
        => !string.IsNullOrWhiteSpace(await tokenStore.GetTokenAsync());

    public override async Task<AuthenticationState> GetAuthenticationStateAsync()
    {
        var token = await tokenStore.GetRawTokenAsync();

        if (string.IsNullOrWhiteSpace(token))
            return Unauthenticated();

        var claims = ParseClaimsFromJwt(token);

        return new AuthenticationState(new ClaimsPrincipal(new ClaimsIdentity(claims, "jwt")));
    }

    public async Task<bool> Login(LoginRequest request)
    {
        var (data, success, _) = await api.PostPublicAsync<LoginResponse>(Constants.Api.Login, request);

        if (!success || data is null) return false;

        await tokenStore.SaveAsync(data);

        NotifyAuthenticationStateChanged(GetAuthenticationStateAsync());

        return true;
    }

    public async Task<string?> Register(RegisterRequest request)
    {
        var (_, success, error) = await api.PostPublicAsync<object>(Constants.Api.Register, request);

        return success ? null : (error ?? "No se pudo crear la cuenta. Intenta con otro email.");
    }

    public async Task Logout()
    {
        await tokenStore.ClearAsync();

        NotifyAuthenticationStateChanged(Task.FromResult(Unauthenticated()));
    }

    public async Task<bool> TryRefreshAsync() => await tokenStore.TryRefreshAsync();

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
                foreach (var item in kv.Value.EnumerateArray())
                    claims.Add(new Claim(kv.Key, item.GetString() ?? string.Empty));
            else
                claims.Add(new Claim(kv.Key, kv.Value.ToString()));
        }

        return claims;
    }

    private static byte[] ParseBase64WithoutPadding(string base64)
    {
        base64 = base64.Replace('-', '+').Replace('_', '/');

        base64 = base64.PadRight(base64.Length + (4 - base64.Length % 4) % 4, '=');

        return Convert.FromBase64String(base64);
    }
}
