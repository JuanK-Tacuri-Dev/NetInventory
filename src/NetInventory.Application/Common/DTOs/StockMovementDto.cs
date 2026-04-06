namespace NetInventory.Application.Common.DTOs;

public sealed record StockMovementDto(
    Guid Id,
    Guid ProductId,
    string Type,
    int Quantity,
    DateTime Timestamp,
    string? CreatedBy);
