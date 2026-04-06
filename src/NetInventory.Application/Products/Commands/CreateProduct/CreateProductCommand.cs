namespace NetInventory.Application.Products.Commands.CreateProduct;

public sealed record CreateProductCommand(
    string Name,
    string SKU,
    string Category,
    decimal UnitPrice);
