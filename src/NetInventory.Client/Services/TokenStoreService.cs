using Microsoft.JSInterop;
using NetInventory.Client.Models;

namespace NetInventory.Client.Services;

public sealed class TokenStoreService(Lazy<ApiClientService> api, IJSRuntime js)
{
    private static readonly TimeSpan RefreshThreshold = Constants.Auth.RefreshThreshold;


    public async Task<string?> GetTokenAsync()
    {
        var token = await GetRawTokenAsync();
        if (string.IsNullOrWhiteSpace(token)) return null;

        var expiresAtStr = await js.InvokeAsync<string?>("localStorage.getItem", Constants.LocalStorage.ExpiresAtKey);
        if (DateTime.TryParse(expiresAtStr, null, System.Globalization.DateTimeStyles.RoundtripKind, out var expiresAt)
            && DateTime.UtcNow >= expiresAt - RefreshThreshold)
        {
            var refreshed = await TryRefreshAsync();

            if (!refreshed) return null;

            return await GetRawTokenAsync();
        }

        return token;
    }

    public async Task<string?> GetRawTokenAsync()
        => await js.InvokeAsync<string?>("localStorage.getItem", Constants.LocalStorage.TokenKey);

    public async Task<bool> TryRefreshAsync()
    {
        try
        {
            var refreshToken = await js.InvokeAsync<string?>("localStorage.getItem", Constants.LocalStorage.RefreshTokenKey);
            if (string.IsNullOrWhiteSpace(refreshToken)) return false;

            var (data, success, _) = await api.Value.PostPublicAsync<LoginResponse>(
                Constants.Api.Refresh, new { refreshToken });

            if (!success || data is null) { await ClearAsync(); return false; }

            await SaveAsync(data);

            return true;
        }
        catch { return false; }
    }

    public async Task SaveAsync(LoginResponse data)
    {
        await js.InvokeVoidAsync("localStorage.setItem", Constants.LocalStorage.TokenKey, data.Token);
        await js.InvokeVoidAsync("localStorage.setItem", Constants.LocalStorage.ExpiresAtKey, data.ExpiresAt.ToString("O"));
        if (!string.IsNullOrEmpty(data.RefreshToken))
            await js.InvokeVoidAsync("localStorage.setItem", Constants.LocalStorage.RefreshTokenKey, data.RefreshToken);
    }

    public async Task ClearAsync()
    {
        await js.InvokeVoidAsync("localStorage.removeItem", Constants.LocalStorage.TokenKey);
        await js.InvokeVoidAsync("localStorage.removeItem", Constants.LocalStorage.ExpiresAtKey);
        await js.InvokeVoidAsync("localStorage.removeItem", Constants.LocalStorage.RefreshTokenKey);
    }
}
