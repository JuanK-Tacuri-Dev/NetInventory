using System.Net;
using NetInventory.Client.Models;

namespace NetInventory.Client.Services;

public sealed class ProductService(ApiClientService api)
{
    public async Task<List<ProductModel>> GetAllAsync(string? categoryCode = null, bool lowStockOnly = false)
    {
        var query = $"{Constants.Api.Products}?lowStockOnly={lowStockOnly}";
        if (!string.IsNullOrWhiteSpace(categoryCode))
            query += $"&categoryCode={Uri.EscapeDataString(categoryCode)}";

        return await api.GetAsync<List<ProductModel>>(query) ?? [];
    }

    public async Task<PagedResult<ProductModel>> GetPagedAsync(
        string? searchName    = null,
        string? searchSku     = null,
        string? searchCategory = null,
        string? searchStock   = null,
        string? searchPrice   = null,
        string[]? categoryCodes = null,
        bool lowStockOnly     = false,
        int page              = 1,
        int pageSize          = 10)
    {
        var body = new
        {
            searchName, searchSku, searchCategory, searchStock, searchPrice,
            categoryCodes, lowStockOnly, page, pageSize
        };
        return await api.PostAsync<PagedResult<ProductModel>>(Constants.Api.ProductsPaged, body)
               ?? new PagedResult<ProductModel>();
    }

    public async Task<ProductModel?> GetByIdAsync(Guid id)
        => await api.GetAsync<ProductModel>($"{Constants.Api.Products}/{id}");

    public async Task<ProductModel?> CreateAsync(CreateProductRequest request)
        => await api.PostAsync<ProductModel>(Constants.Api.Products, request);

    public async Task<ProductModel?> UpdateAsync(Guid id, UpdateProductRequest request)
        => await api.PutAsync<ProductModel>($"{Constants.Api.Products}/{id}", request);

    public async Task<bool> DeleteAsync(Guid id)
        => await api.DeleteAsync($"{Constants.Api.Products}/{id}");

    public async Task<(StockMovementModel? Data, string? Error)> RegisterMovementAsync(Guid productId, RegisterMovementRequest request)
    {
        var (data, status, error) = await api.PostDetailedAsync<StockMovementModel>(
            $"{Constants.Api.Products}/{productId}/movements", request);

        if (data is null)
        {
            var msg = status == HttpStatusCode.NotFound
                ? "Producto no encontrado. Verifica que tu sesión sea correcta."
                : error ?? "No se pudo registrar el movimiento.";
            return (null, msg);
        }

        return (data, null);
    }

    public async Task<List<StockMovementModel>> GetMovementsAsync(Guid productId)
        => await api.GetAsync<List<StockMovementModel>>($"{Constants.Api.Products}/{productId}/movements") ?? [];
}
