using NetInventory.Client.Models;

namespace NetInventory.Client.Services;

public sealed class ErrorLogService(ApiClientService api)
{
    public async Task<PagedResult<ErrorLogModel>> GetPagedAsync(int page = 1, int pageSize = 10)
        => await api.GetAsync<PagedResult<ErrorLogModel>>(
               $"{Constants.Api.ErrorLogs}?page={page}&pageSize={pageSize}")
           ?? new PagedResult<ErrorLogModel>();

    public async Task<bool> SimulateErrorAsync(string message)
    {
        var (_, status, _) = await api.PostDetailedAsync<object>(
            Constants.Api.DiagnosticsError, new { message });
        return status == System.Net.HttpStatusCode.InternalServerError
            || ((int)status >= 200 && (int)status < 300);
    }
}
