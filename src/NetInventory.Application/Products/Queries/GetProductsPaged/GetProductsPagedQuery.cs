using NetInventory.Application.Common;
using NetInventory.Application.Common.DTOs;
using NetInventory.Domain.Common;

namespace NetInventory.Application.Products.Queries.GetProductsPaged;

public sealed record GetProductsPagedQuery(
    string? SearchName,
    string? SearchSku,
    string? SearchCategory,
    string? SearchStock,
    string? SearchPrice,
    string[] CategoryCodes,
    bool LowStockOnly,
    int Page,
    int PageSize) : IQuery<Result<PagedResult<ProductListItem>>>;
