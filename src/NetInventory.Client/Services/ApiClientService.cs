using System.Net;
using System.Net.Http.Headers;
using System.Net.Http.Json;
using NetInventory.Client.Models;

namespace NetInventory.Client.Services;

public sealed class ApiClientService(HttpClient http, TokenStoreService tokenStore)
{
    private async Task<HttpRequestMessage> BuildAsync(HttpMethod method, string url, object? body = null)
    {
        var token = await tokenStore.GetTokenAsync();

        var request = new HttpRequestMessage(method, url);

        if (!string.IsNullOrWhiteSpace(token))
            request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", token);

        if (body is not null)
            request.Content = JsonContent.Create(body);

        return request;
    }

    public async Task<T?> GetAsync<T>(string url) where T : class
    {
        try
        {
            using var req = await BuildAsync(HttpMethod.Get, url);
            using var res = await http.SendAsync(req);
            if (!res.IsSuccessStatusCode) return default;
            var result = await res.Content.ReadFromJsonAsync<ApiResponse<T>>();
            return result is { Success: true } ? result.Data : default;
        }
        catch { return default; }
    }

    public async Task<T?> PostAsync<T>(string url, object body) where T : class
    {
        try
        {
            using var req = await BuildAsync(HttpMethod.Post, url, body);
            using var res = await http.SendAsync(req);
            if (!res.IsSuccessStatusCode) return default;
            var result = await res.Content.ReadFromJsonAsync<ApiResponse<T>>();
            return result is { Success: true } ? result.Data : default;
        }
        catch { return default; }
    }

    public async Task<bool> PostAsync(string url, object body)
    {
        try
        {
            using var req = await BuildAsync(HttpMethod.Post, url, body);
            using var res = await http.SendAsync(req);
            return res.IsSuccessStatusCode;
        }
        catch { return false; }
    }

    public async Task<(T? Data, HttpStatusCode Status, string? Error)> PostDetailedAsync<T>(string url, object body) where T : class
    {
        try
        {
            using var req = await BuildAsync(HttpMethod.Post, url, body);
            using var res = await http.SendAsync(req);
            if (!res.IsSuccessStatusCode)
            {
                var err = await res.Content.ReadFromJsonAsync<ApiResponse<T>>();
                return (default, res.StatusCode, err?.Error);
            }
            var result = await res.Content.ReadFromJsonAsync<ApiResponse<T>>();
            return (result?.Data, res.StatusCode, null);
        }
        catch { return (default, HttpStatusCode.ServiceUnavailable, "Error de conexión."); }
    }

    public async Task<T?> PutAsync<T>(string url, object body) where T : class
    {
        try
        {
            using var req = await BuildAsync(HttpMethod.Put, url, body);
            using var res = await http.SendAsync(req);
            if (!res.IsSuccessStatusCode) return default;
            var result = await res.Content.ReadFromJsonAsync<ApiResponse<T>>();
            return result is { Success: true } ? result.Data : default;
        }
        catch { return default; }
    }

    public async Task<bool> PutAsync(string url, object body)
    {
        try
        {
            using var req = await BuildAsync(HttpMethod.Put, url, body);
            using var res = await http.SendAsync(req);
            return res.IsSuccessStatusCode;
        }
        catch { return false; }
    }

    public async Task<bool> DeleteAsync(string url)
    {
        try
        {
            using var req = await BuildAsync(HttpMethod.Delete, url);
            using var res = await http.SendAsync(req);
            return res.IsSuccessStatusCode;
        }
        catch { return false; }
    }

    public async Task<bool> PatchAsync(string url, object? body = null)
    {
        try
        {
            using var req = await BuildAsync(HttpMethod.Patch, url, body);
            using var res = await http.SendAsync(req);
            return res.IsSuccessStatusCode;
        }
        catch { return false; }
    }

    // ── PUBLIC (sin token) — exclusivo para AuthService ────────────────────

    /// <summary>
    /// POST sin cabecera de autenticación.
    /// Usado por AuthService para login/register/refresh (endpoints públicos).
    /// Retorna (Data, Success, Error) para que AuthService maneje cada caso.
    /// </summary>
    public async Task<(T? Data, bool Success, string? Error)> PostPublicAsync<T>(string url, object body) where T : class
    {
        try
        {
            using var req = new HttpRequestMessage(HttpMethod.Post, url)
            {
                Content = JsonContent.Create(body)
            };
            using var res = await http.SendAsync(req);

            if (!res.IsSuccessStatusCode)
            {
                var err = await res.Content.ReadFromJsonAsync<ApiResponse>();
                return (default, false, err?.Error);
            }

            var result = await res.Content.ReadFromJsonAsync<ApiResponse<T>>();
            return (result?.Data, result?.Success ?? false, null);
        }
        catch { return (default, false, "Error de conexión con el servidor."); }
    }
}
