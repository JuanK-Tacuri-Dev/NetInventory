namespace NetInventory.Api.Requests.Products;

public sealed record GetProductsPagedRequest(
    string? SearchName = null,
    string? SearchSku = null,
    string? SearchCategory = null,
    string? SearchStock = null,
    string? SearchPrice = null,
    string[]? CategoryCodes = null,
    bool LowStockOnly = false,
    int Page = 1,
    int PageSize = 10);
