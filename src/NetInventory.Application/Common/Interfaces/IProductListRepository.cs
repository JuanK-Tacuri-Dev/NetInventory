using NetInventory.Application.Common.DTOs;

namespace NetInventory.Application.Common.Interfaces;

public interface IProductListRepository
{
    Task<PagedResult<ProductListItem>> GetPagedAsync(
        string? category,
        bool lowStockOnly,
        int threshold,
        int page,
        int pageSize,
        CancellationToken ct = default);
}
