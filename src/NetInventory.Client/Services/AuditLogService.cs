using System.Net.Http.Headers;
using System.Net.Http.Json;
using NetInventory.Client.Models;

namespace NetInventory.Client.Services;

public class AuditLogService(HttpClient http, AuthService auth)
{
    private async Task SetAuthHeader()
    {
        var token = await auth.GetToken();
        http.DefaultRequestHeaders.Authorization =
            string.IsNullOrWhiteSpace(token) ? null : new AuthenticationHeaderValue("Bearer", token);
    }

    public async Task<PagedResult<AuditLogModel>> GetPagedAsync(int page = 1, int pageSize = 50)
    {
        try
        {
            await SetAuthHeader();
            var result = await http.GetFromJsonAsync<ApiResponse<PagedResult<AuditLogModel>>>(
                $"{Constants.Api.AuditLogs}?page={page}&pageSize={pageSize}");
            return result?.Data ?? new PagedResult<AuditLogModel>();
        }
        catch
        {
            return new PagedResult<AuditLogModel>();
        }
    }

    public async Task<AuditLogModel?> GetByCorrelationIdAsync(string correlationId)
    {
        try
        {
            await SetAuthHeader();
            var result = await http.GetFromJsonAsync<ApiResponse<AuditLogModel>>(
                $"{Constants.Api.AuditLogs}/{Uri.EscapeDataString(correlationId)}");
            return result?.Data;
        }
        catch
        {
            return null;
        }
    }
}
