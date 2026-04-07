namespace NetInventory.Application.Common.Interfaces;

public interface ICacheService
{
    Task<T?> GetOrSetAsync<T>(string key, Func<Task<T?>> factory, TimeSpan expiration);
    void Remove(string key);
}
