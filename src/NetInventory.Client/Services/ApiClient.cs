using System.Net;
using System.Net.Http.Headers;
using System.Net.Http.Json;
using NetInventory.Client.Models;

namespace NetInventory.Client.Services;

/// <summary>
/// Centraliza toda la comunicación HTTP autenticada con la API.
/// Usa headers por mensaje (no DefaultRequestHeaders) para evitar race conditions en async/await.
/// </summary>
public sealed class ApiClient(HttpClient http, AuthService auth)
{
    private async Task<HttpRequestMessage> BuildAsync(HttpMethod method, string url, object? body = null)
    {
        var token = await auth.GetToken();
        var request = new HttpRequestMessage(method, url);

        if (!string.IsNullOrWhiteSpace(token))
            request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", token);

        if (body is not null)
            request.Content = JsonContent.Create(body);

        return request;
    }

    // ── GET ────────────────────────────────────────────────────────────────

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

    // ── POST ───────────────────────────────────────────────────────────────

    /// <summary>POST que retorna T? desde ApiResponse&lt;T&gt;.Data.</summary>
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

    /// <summary>POST que retorna bool (solo interesa si fue exitoso).</summary>
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

    /// <summary>
    /// POST detallado: retorna Data + StatusCode + Error.
    /// Útil cuando el servicio necesita distinguir tipos de error (ej. 404 vs 400).
    /// </summary>
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

    // ── PUT ────────────────────────────────────────────────────────────────

    /// <summary>PUT que retorna T? desde ApiResponse&lt;T&gt;.Data.</summary>
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

    /// <summary>PUT que retorna bool (solo interesa si fue exitoso).</summary>
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

    // ── DELETE ─────────────────────────────────────────────────────────────

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

    // ── PATCH ──────────────────────────────────────────────────────────────

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
}
