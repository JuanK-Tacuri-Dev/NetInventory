using NetInventory.Application.Common;
using NetInventory.Application.Common.DTOs;
using NetInventory.Application.Common.Interfaces;
using NetInventory.Domain.Common;

namespace NetInventory.Application.Products.Queries.GetProductsPaged;

public sealed class GetProductsPagedQueryHandler(IProductListRepository repository, ICurrentUserService currentUserService)
    : IQueryHandler<GetProductsPagedQuery, Result<PagedResult<ProductListItem>>>
{
    public async Task<Result<PagedResult<ProductListItem>>> HandleAsync(
        GetProductsPagedQuery query, CancellationToken ct = default)
    {
        var ownerId = currentUserService.GetCurrentUserId();
        var result = await repository.GetPagedAsync(
            ownerId,
            query.SearchName,
            query.SearchSku,
            query.SearchCategory,
            query.SearchStock,
            query.SearchPrice,
            query.CategoryCodes,
            query.LowStockOnly,
            query.Page,
            query.PageSize,
            ct);

        return Result.Success(result);
    }
}
