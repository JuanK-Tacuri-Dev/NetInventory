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

    public async Task<List<ProductModel>> GetAllAsync(string? categoryCode = null, bool lowStockOnly = false)
    {
        try
        {
            await SetAuthHeader();
            var query = $"{Constants.Api.Products}?lowStockOnly={lowStockOnly}";
            if (!string.IsNullOrWhiteSpace(categoryCode))
                query += $"&categoryCode={Uri.EscapeDataString(categoryCode)}";

            var result = await _http.GetFromJsonAsync<ApiResponse<List<ProductModel>>>(query);
            return result?.Data ?? [];
        }
        catch
        {
            return [];
        }
    }

    public async Task<PagedResult<ProductModel>> GetPagedAsync(
        string? searchName = null,
        string? searchSku = null,
        string? searchCategory = null,
        string? searchStock = null,
        string? searchPrice = null,
        string[]? categoryCodes = null,
        bool lowStockOnly = false,
        int page = 1,
        int pageSize = 10)
    {
        try
        {
            await SetAuthHeader();
            var body = new
            {
                searchName, searchSku, searchCategory, searchStock, searchPrice,
                categoryCodes, lowStockOnly, page, pageSize
            };
            var response = await _http.PostAsJsonAsync(Constants.Api.ProductsPaged, body);
            if (!response.IsSuccessStatusCode) return new PagedResult<ProductModel>();
            var result = await response.Content.ReadFromJsonAsync<ApiResponse<PagedResult<ProductModel>>>();
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
            var result = await _http.GetFromJsonAsync<ApiResponse<ProductModel>>($"{Constants.Api.Products}/{id}");
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
            var response = await _http.PostAsJsonAsync(Constants.Api.Products, request);
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
            var response = await _http.PutAsJsonAsync($"{Constants.Api.Products}/{id}", request);
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
            var response = await _http.DeleteAsync($"{Constants.Api.Products}/{id}");
            return response.IsSuccessStatusCode;
        }
        catch
        {
            return false;
        }
    }

    public async Task<(StockMovementModel? Data, string? Error)> RegisterMovementAsync(Guid productId, RegisterMovementRequest request)
    {
        try
        {
            await SetAuthHeader();
            var response = await _http.PostAsJsonAsync($"{Constants.Api.Products}/{productId}/movements", request);
            if (!response.IsSuccessStatusCode)
            {
                var err = await response.Content.ReadFromJsonAsync<ApiResponse<object>>();
                var msg = response.StatusCode == System.Net.HttpStatusCode.NotFound
                    ? "Producto no encontrado. Verifica que tu sesión sea correcta."
                    : err?.Error ?? "No se pudo registrar el movimiento.";
                return (null, msg);
            }
            var result = await response.Content.ReadFromJsonAsync<ApiResponse<StockMovementModel>>();
            return (result?.Data, null);
        }
        catch
        {
            return (null, "Error de conexión. Intenta de nuevo.");
        }
    }

    public async Task<List<StockMovementModel>> GetMovementsAsync(Guid productId)
    {
        try
        {
            await SetAuthHeader();
            var result = await _http.GetFromJsonAsync<ApiResponse<List<StockMovementModel>>>($"{Constants.Api.Products}/{productId}/movements");
            return result?.Data ?? [];
        }
        catch
        {
            return [];
        }
    }
}
