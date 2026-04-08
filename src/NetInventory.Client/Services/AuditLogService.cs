using NetInventory.Client.Models;

namespace NetInventory.Client.Services;

public sealed class AuditLogService(ApiClient api)
{
    public async Task<PagedResult<AuditLogModel>> GetPagedAsync(int page = 1, int pageSize = 50)
        => await api.GetAsync<PagedResult<AuditLogModel>>(
               $"{Constants.Api.AuditLogs}?page={page}&pageSize={pageSize}")
           ?? new PagedResult<AuditLogModel>();

    public async Task<AuditLogModel?> GetByCorrelationIdAsync(string correlationId)
        => await api.GetAsync<AuditLogModel>(
               $"{Constants.Api.AuditLogs}/{Uri.EscapeDataString(correlationId)}");
}
