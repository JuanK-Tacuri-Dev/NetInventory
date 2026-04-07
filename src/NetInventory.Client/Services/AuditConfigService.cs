using NetInventory.Client.Models;
using System.Net.Http.Headers;
using System.Net.Http.Json;

namespace NetInventory.Client.Services;

public class AuditConfigService(HttpClient http, AuthService auth)
{
    private async Task SetAuthHeader()
    {
        var token = await auth.GetToken();
        http.DefaultRequestHeaders.Authorization =
            string.IsNullOrWhiteSpace(token) ? null : new AuthenticationHeaderValue("Bearer", token);
    }

    public async Task<List<AuditConfigModel>> GetAllAsync()
    {
        try
        {
            await SetAuthHeader();
            var result = await http.GetFromJsonAsync<ApiResponse<List<AuditConfigModel>>>(Constants.Api.AuditConfigs);
            return result?.Data ?? [];
        }
        catch
        {
            return [];
        }
    }

    public async Task<bool> CreateAsync(AuditConfigModel model)
    {
        try
        {
            await SetAuthHeader();
            var response = await http.PostAsJsonAsync(Constants.Api.AuditConfigs, new
            {
                method = model.Method,
                urlPattern = model.UrlPattern,
                description = model.Description
            });
            return response.IsSuccessStatusCode;
        }
        catch { return false; }
    }

    public async Task<bool> UpdateAsync(AuditConfigModel model)
    {
        try
        {
            await SetAuthHeader();
            var response = await http.PutAsJsonAsync($"{Constants.Api.AuditConfigs}/{model.Id}", new
            {
                id = model.Id,
                method = model.Method,
                urlPattern = model.UrlPattern,
                description = model.Description
            });
            return response.IsSuccessStatusCode;
        }
        catch { return false; }
    }

    public async Task<bool> DeleteAsync(Guid id)
    {
        try
        {
            await SetAuthHeader();
            var response = await http.DeleteAsync($"{Constants.Api.AuditConfigs}/{id}");
            return response.IsSuccessStatusCode;
        }
        catch { return false; }
    }

    public async Task<bool> ToggleAsync(Guid id)
    {
        try
        {
            await SetAuthHeader();
            var response = await http.PatchAsync($"{Constants.Api.AuditConfigs}/{id}/toggle", null);
            return response.IsSuccessStatusCode;
        }
        catch { return false; }
    }

    public async Task InvalidateCacheAsync()
    {
        try
        {
            await SetAuthHeader();
            await http.DeleteAsync(Constants.Api.AuditConfigsCache);
        }
        catch { /* best-effort */ }
    }
}
