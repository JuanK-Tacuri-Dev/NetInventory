using Microsoft.Extensions.Caching.Memory;
using NetInventory.Application.Common.Interfaces;

namespace NetInventory.Infrastructure.Services;

public sealed class CacheService(IMemoryCache cache) : ICacheService
{
    public async Task<T?> GetOrSetAsync<T>(string key, Func<Task<T?>> factory, TimeSpan expiration)
    {
        if (cache.TryGetValue(key, out T? cached))
            return cached;

        var value = await factory();
        if (value is null) return default;

        cache.Set(key, value, expiration);
        return value;
    }

    public void Remove(string key) => cache.Remove(key);
}
