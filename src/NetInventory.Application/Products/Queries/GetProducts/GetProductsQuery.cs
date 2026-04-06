namespace NetInventory.Application.Products.Queries.GetProducts;

public sealed record GetProductsQuery(
    string? Category,
    bool LowStockOnly,
    int Threshold = 10);
