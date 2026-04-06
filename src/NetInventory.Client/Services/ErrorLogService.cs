using System.Net.Http.Headers;
using System.Net.Http.Json;
using NetInventory.Client.Models;

namespace NetInventory.Client.Services;

public class ErrorLogService(HttpClient http, AuthService auth)
{
    private async Task SetAuthHeader()
    {
        var token = await auth.GetToken();
        http.DefaultRequestHeaders.Authorization =
            string.IsNullOrWhiteSpace(token) ? null : new AuthenticationHeaderValue("Bearer", token);
    }

    public async Task<List<ErrorLogModel>> GetAllAsync()
    {
        try
        {
            await SetAuthHeader();
            var result = await http.GetFromJsonAsync<ApiResponse<PagedResult<ErrorLogModel>>>("/api/error-logs?pageSize=500");
            return result?.Data?.Items?.ToList() ?? [];
        }
        catch
        {
            return [];
        }
    }
}
