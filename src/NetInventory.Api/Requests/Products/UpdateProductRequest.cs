namespace NetInventory.Api.Requests.Products;

public sealed record UpdateProductRequest(
    string Name, 
    int CategoryTableId, 
    string CategoryCode,
    decimal UnitPrice, 
    int MinStock, 
    int MaxStock);
