namespace NetInventory.Api.Requests.Products;

public sealed record CreateProductRequest(
    string Name,
    string SKU,
    int CategoryTableId,
    string CategoryCode,
    decimal UnitPrice,
    int MinStock,
    int MaxStock);
