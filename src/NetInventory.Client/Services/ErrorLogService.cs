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

    public async Task<PagedResult<ErrorLogModel>> GetPagedAsync(int page = 1, int pageSize = 10)
    {
        try
        {
            await SetAuthHeader();
            var result = await http.GetFromJsonAsync<ApiResponse<PagedResult<ErrorLogModel>>>($"{Constants.Api.ErrorLogs}?page={page}&pageSize={pageSize}");
            return result?.Data ?? new PagedResult<ErrorLogModel>();
        }
        catch
        {
            return new PagedResult<ErrorLogModel>();
        }
    }

    public async Task<bool> SimulateErrorAsync(string message)
    {
        try
        {
            await SetAuthHeader();
            var response = await http.PostAsJsonAsync(Constants.Api.DiagnosticsError, new { message });
            // Un 500 es el resultado esperado — confirmamos que la solicitud llegó
            return response.StatusCode == System.Net.HttpStatusCode.InternalServerError
                || response.IsSuccessStatusCode;
        }
        catch
        {
            return false;
        }
    }
}
