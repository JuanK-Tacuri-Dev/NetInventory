using NetInventory.Application.Common;
using NetInventory.Application.Common.DTOs;
using NetInventory.Domain.Common;

namespace NetInventory.Application.Products.Queries.GetProducts;

public sealed record GetProductsQuery(
    string? CategoryCode,
    bool LowStockOnly) : IQuery<Result<IEnumerable<ProductDto>>>;
