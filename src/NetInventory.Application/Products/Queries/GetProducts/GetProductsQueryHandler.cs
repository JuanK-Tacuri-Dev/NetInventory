using NetInventory.Application.Common.DTOs;
using NetInventory.Domain.Common;
using NetInventory.Domain.Entities;
using NetInventory.Domain.Interfaces;

namespace NetInventory.Application.Products.Queries.GetProducts;

public sealed class GetProductsQueryHandler(IProductRepository productRepository)
{
    public async Task<Result<IEnumerable<ProductDto>>> HandleAsync(GetProductsQuery query, CancellationToken ct = default)
    {
        var products = await productRepository.GetAllAsync(query.Category, query.LowStockOnly, query.Threshold, ct);
        return Result.Success(products.Select(p => ToDto(p, query.Threshold)));
    }

    private static ProductDto ToDto(Product p, int threshold) =>
        new(p.Id, p.Name, p.SKU.Value, p.Category, p.QuantityInStock, p.UnitPrice.Amount, p.CreatedAt, p.IsLowStock(threshold));
}
