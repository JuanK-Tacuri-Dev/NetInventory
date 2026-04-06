namespace NetInventory.Application.Common.DTOs;

public sealed record ProductFilterDto(
    string? Category,
    bool LowStockOnly,
    int Threshold = 10);
