namespace NetInventory.Application.Products.Queries.GetProductsPaged;

public sealed record GetProductsPagedQuery(
    string? Category,
    bool LowStockOnly,
    int Threshold,
    int Page,
    int PageSize);
