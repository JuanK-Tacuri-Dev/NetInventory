namespace NetInventory.Application.Common.DTOs;

public sealed record ProductDto(
    Guid Id,
    string Name,
    string SKU,
    int CategoryTableId,
    string CategoryCode,
    string CategoryDescription,
    int QuantityInStock,
    int MinStock,
    int MaxStock,
    decimal UnitPrice,
    DateTime CreatedAt,
    bool IsLowStock);
