using NetInventory.Application.Common.DTOs;
using NetInventory.Application.Common.Interfaces;
using NetInventory.Domain.Common;

namespace NetInventory.Application.Products.Queries.GetProductsPaged;

public sealed class GetProductsPagedQueryHandler(IProductListRepository repository)
{
    public async Task<Result<PagedResult<ProductListItem>>> HandleAsync(
        GetProductsPagedQuery query, CancellationToken ct = default)
    {
        var result = await repository.GetPagedAsync(
            query.Category,
            query.LowStockOnly,
            query.Threshold,
            query.Page,
            query.PageSize,
            ct);

        return Result.Success(result);
    }
}
