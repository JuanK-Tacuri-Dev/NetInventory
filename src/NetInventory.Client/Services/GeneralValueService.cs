using NetInventory.Client.Models;

namespace NetInventory.Client.Services;

public sealed class GeneralValueService(ApiClient api)
{
    private readonly Dictionary<int, List<GeneralValueModel>> _cache = [];

    public async Task<List<GeneralValueModel>> GetByTableIdAsync(int tableId)
    {
        if (_cache.TryGetValue(tableId, out var cached))
            return cached;

        var data = await api.GetAsync<List<GeneralValueModel>>(
                       $"{Constants.Api.GeneralValues}?tableId={tableId}")
                   ?? [];

        _cache[tableId] = data;
        return data;
    }

    public void InvalidateTable(int tableId) => _cache.Remove(tableId);
}
