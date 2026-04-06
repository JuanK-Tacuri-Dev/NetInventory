using System.Net.Http.Headers;
using System.Net.Http.Json;
using NetInventory.Client.Models;

namespace NetInventory.Client.Services;

public class ProductService
{
    private readonly HttpClient _http;
    private readonly AuthService _auth;

    public ProductService(HttpClient http, AuthService auth)
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

    public async Task<List<ProductModel>> GetAllAsync(string? category = null, bool lowStockOnly = false, int threshold = 10)
    {
        try
        {
            await SetAuthHeader();
            var query = $"/api/products?lowStockOnly={lowStockOnly}&threshold={threshold}";
            if (!string.IsNullOrWhiteSpace(category))
                query += $"&category={Uri.EscapeDataString(category)}";

            var result = await _http.GetFromJsonAsync<ApiResponse<List<ProductModel>>>(query);
            return result?.Data ?? [];
        }
        catch
        {
            return [];
        }
    }

    public async Task<PagedResult<ProductModel>> GetPagedAsync(
        string? category = null,
        bool lowStockOnly = false,
        int threshold = 10,
        int page = 1,
        int pageSize = 10)
    {
        try
        {
            await SetAuthHeader();
            var query = $"/api/products/paged?lowStockOnly={lowStockOnly}&threshold={threshold}&page={page}&pageSize={pageSize}";
            if (!string.IsNullOrWhiteSpace(category))
                query += $"&category={Uri.EscapeDataString(category)}";

            var result = await _http.GetFromJsonAsync<ApiResponse<PagedResult<ProductModel>>>(query);
            return result?.Data ?? new PagedResult<ProductModel>();
        }
        catch
        {
            return new PagedResult<ProductModel>();
        }
    }

    public async Task<ProductModel?> GetByIdAsync(Guid id)
    {
        try
        {
            await SetAuthHeader();
            var result = await _http.GetFromJsonAsync<ApiResponse<ProductModel>>($"/api/products/{id}");
            return result?.Data;
        }
        catch
        {
            return null;
        }
    }

    public async Task<ProductModel?> CreateAsync(CreateProductRequest request)
    {
        try
        {
            await SetAuthHeader();
            var response = await _http.PostAsJsonAsync("/api/products", request);
            if (!response.IsSuccessStatusCode) return null;
            var result = await response.Content.ReadFromJsonAsync<ApiResponse<ProductModel>>();
            return result?.Data;
        }
        catch
        {
            return null;
        }
    }

    public async Task<ProductModel?> UpdateAsync(Guid id, UpdateProductRequest request)
    {
        try
        {
            await SetAuthHeader();
            var response = await _http.PutAsJsonAsync($"/api/products/{id}", request);
            if (!response.IsSuccessStatusCode) return null;
            var result = await response.Content.ReadFromJsonAsync<ApiResponse<ProductModel>>();
            return result?.Data;
        }
        catch
        {
            return null;
        }
    }

    public async Task<bool> DeleteAsync(Guid id)
    {
        try
        {
            await SetAuthHeader();
            var response = await _http.DeleteAsync($"/api/products/{id}");
            return response.IsSuccessStatusCode;
        }
        catch
        {
            return false;
        }
    }

    public async Task<StockMovementModel?> RegisterMovementAsync(Guid productId, RegisterMovementRequest request)
    {
        try
        {
            await SetAuthHeader();
            var response = await _http.PostAsJsonAsync($"/api/products/{productId}/movements", request);
            if (!response.IsSuccessStatusCode) return null;
            var result = await response.Content.ReadFromJsonAsync<ApiResponse<StockMovementModel>>();
            return result?.Data;
        }
        catch
        {
            return null;
        }
    }

    public async Task<List<StockMovementModel>> GetMovementsAsync(Guid productId)
    {
        try
        {
            await SetAuthHeader();
            var result = await _http.GetFromJsonAsync<ApiResponse<List<StockMovementModel>>>($"/api/products/{productId}/movements");
            return result?.Data ?? [];
        }
        catch
        {
            return [];
        }
    }
}
