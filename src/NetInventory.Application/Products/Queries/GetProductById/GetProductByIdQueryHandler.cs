using Mapster;
using NetInventory.Application.Common;
using NetInventory.Application.Common.DTOs;
using NetInventory.Application.Common.Interfaces;
using NetInventory.Domain.Common;
using NetInventory.Domain.Interfaces;

namespace NetInventory.Application.Products.Queries.GetProductById;

public sealed class GetProductByIdQueryHandler(IProductRepository productRepository, ICurrentUserService currentUserService)
    : IQueryHandler<GetProductByIdQuery, Result<ProductDto>>
{
    public async Task<Result<ProductDto>> HandleAsync(GetProductByIdQuery query, CancellationToken ct = default)
    {
        var ownerId = currentUserService.GetCurrentUserId();
        var product = await productRepository.GetByIdAsync(query.Id, ownerId, ct);
        if (product is null)
            return Result.Failure<ProductDto>(Error.Product.NotFound);

        return Result.Success(product.Adapt<ProductDto>());
    }
}
