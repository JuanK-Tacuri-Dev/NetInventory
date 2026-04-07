using NetInventory.Application.Common.DTOs;

namespace NetInventory.Application.Common.Interfaces;

public interface IProductListRepository
{
    Task<PagedResult<ProductListItem>> GetPagedAsync(
        string ownerId,
        string? searchName,
        string? searchSku,
        string? searchCategory,
        string? searchStock,
        string? searchPrice,
        string[] categoryCodes,
        bool lowStockOnly,
        int page,
        int pageSize,
        CancellationToken ct = default);
}
