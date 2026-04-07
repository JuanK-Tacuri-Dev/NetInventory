using NetInventory.Application.Common;
using NetInventory.Application.Common.DTOs;
using NetInventory.Domain.Common;

namespace NetInventory.Application.Products.Queries.GetProductById;

public sealed record GetProductByIdQuery(Guid Id) : IQuery<Result<ProductDto>>;
