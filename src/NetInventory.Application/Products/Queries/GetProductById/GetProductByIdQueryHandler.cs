using NetInventory.Application.Common.DTOs;
using NetInventory.Domain.Common;
using NetInventory.Domain.Entities;
using NetInventory.Domain.Interfaces;

namespace NetInventory.Application.Products.Queries.GetProductById;

public sealed class GetProductByIdQueryHandler(IProductRepository productRepository)
{
    public async Task<Result<ProductDto>> HandleAsync(GetProductByIdQuery query, CancellationToken ct = default)
    {
        var product = await productRepository.GetByIdAsync(query.Id, ct);
        if (product is null)
            return Result.Failure<ProductDto>(Error.ProductNotFound);

        return Result.Success(ToDto(product));
    }

    private static ProductDto ToDto(Product p) =>
        new(p.Id, p.Name, p.SKU.Value, p.Category, p.QuantityInStock, p.UnitPrice.Amount, p.CreatedAt, p.IsLowStock());
}
