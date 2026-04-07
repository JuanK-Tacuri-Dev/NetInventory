using System.Net.Http.Headers;
using System.Net.Http.Json;
using NetInventory.Client.Models;

namespace NetInventory.Client.Services;

public class GeneralValueService
{
    private readonly HttpClient _http;
    private readonly AuthService _auth;
    private readonly Dictionary<int, List<GeneralValueModel>> _cache = [];

    public GeneralValueService(HttpClient http, AuthService auth)
    {
        _http = http;
        _auth = auth;
    }

    private async Task SetAuthHeader()
    {
        var token = await _auth.GetToken();
        _http.DefaultRequestHeaders.Authorization =
            string.IsNullOrWhiteSpace(token)
                ? null
                : new AuthenticationHeaderValue("Bearer", token);
    }

    public async Task<List<GeneralValueModel>> GetByTableIdAsync(int tableId)
    {
        if (_cache.TryGetValue(tableId, out var cached))
            return cached;

        try
        {
            await SetAuthHeader();
            var result = await _http.GetFromJsonAsync<ApiResponse<List<GeneralValueModel>>>($"{Constants.Api.GeneralValues}?tableId={tableId}");
            var data = result?.Data ?? [];
            _cache[tableId] = data;
            return data;
        }
        catch
        {
            return [];
        }
    }

    public void InvalidateTable(int tableId) => _cache.Remove(tableId);
}
