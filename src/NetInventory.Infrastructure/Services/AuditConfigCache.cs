using Mapster;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.DependencyInjection;
using NetInventory.Application.Common.DTOs;
using NetInventory.Application.Common.Interfaces;
using NetInventory.Domain.Interfaces;

namespace NetInventory.Infrastructure.Services;

public sealed class AuditConfigCache(IMemoryCache cache, IServiceScopeFactory scopeFactory) : IAuditConfigCache
{
    public async Task<IEnumerable<AuditConfigDto>> GetActiveConfigsAsync()
    {
        if (cache.TryGetValue(Constants.Cache.AuditConfigs, out IEnumerable<AuditConfigDto>? cached) && cached is not null)
            return cached;

        using var scope = scopeFactory.CreateScope();
        var repository = scope.ServiceProvider.GetRequiredService<IAuditConfigRepository>();
        var configs = await repository.GetAllAsync();

        var dtos = configs.Adapt<List<AuditConfigDto>>();

        cache.Set(Constants.Cache.AuditConfigs, dtos, Constants.Cache.AuditConfigsTtl);
        return dtos;
    }

    public void Invalidate() => cache.Remove(Constants.Cache.AuditConfigs);
}
