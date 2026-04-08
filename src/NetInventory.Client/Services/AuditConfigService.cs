using NetInventory.Client.Models;

namespace NetInventory.Client.Services;

public sealed class AuditConfigService(ApiClientService api)
{
    public async Task<List<AuditConfigModel>> GetAllAsync()
        => await api.GetAsync<List<AuditConfigModel>>(Constants.Api.AuditConfigs) ?? [];

    public async Task<bool> CreateAsync(AuditConfigModel model)
        => await api.PostAsync(Constants.Api.AuditConfigs, new
        {
            method     = model.Method,
            urlPattern = model.UrlPattern,
            description = model.Description
        });

    public async Task<bool> UpdateAsync(AuditConfigModel model)
        => await api.PutAsync($"{Constants.Api.AuditConfigs}/{model.Id}", new
        {
            id          = model.Id,
            method      = model.Method,
            urlPattern  = model.UrlPattern,
            description = model.Description
        });

    public async Task<bool> DeleteAsync(Guid id)
        => await api.DeleteAsync($"{Constants.Api.AuditConfigs}/{id}");

    public async Task<bool> ToggleAsync(Guid id)
        => await api.PatchAsync($"{Constants.Api.AuditConfigs}/{id}/toggle");

    public async Task InvalidateCacheAsync()
        => await api.DeleteAsync(Constants.Api.AuditConfigsCache);
}
