using Mapster;
using NetInventory.Application.Common;
using NetInventory.Application.Common.DTOs;
using NetInventory.Application.Common.Interfaces;
using NetInventory.Domain.Common;
using NetInventory.Domain.Interfaces;

namespace NetInventory.Application.Products.Queries.GetProducts;

public sealed class GetProductsQueryHandler(IProductRepository productRepository, ICurrentUserService currentUserService)
    : IQueryHandler<GetProductsQuery, Result<IEnumerable<ProductDto>>>
{
    public async Task<Result<IEnumerable<ProductDto>>> HandleAsync(GetProductsQuery query, CancellationToken ct = default)
    {
        var ownerId = currentUserService.GetCurrentUserId();
        var products = await productRepository.GetAllAsync(ownerId, query.CategoryCode, query.LowStockOnly, ct);
        return Result.Success(products.Select(p =>
            p.Adapt<ProductDto>() with { IsLowStock = p.IsLowStock() }));
    }
}
