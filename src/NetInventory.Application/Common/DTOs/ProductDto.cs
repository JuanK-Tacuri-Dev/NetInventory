namespace NetInventory.Application.Common.DTOs;

public sealed record ProductDto(
    Guid Id,
    string Name,
    string SKU,
    string Category,
    int QuantityInStock,
    decimal UnitPrice,
    DateTime CreatedAt,
    bool IsLowStock);
