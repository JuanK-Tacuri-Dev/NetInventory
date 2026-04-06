namespace NetInventory.Application.Products.Commands.UpdateProduct;

public sealed record UpdateProductCommand(
    Guid Id,
    string Name,
    string Category,
    decimal UnitPrice);
