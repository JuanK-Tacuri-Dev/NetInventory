using NetInventory.Application.Common;
using NetInventory.Application.Common.DTOs;
using NetInventory.Domain.Common;

namespace NetInventory.Application.Products.Commands.CreateProduct;

public sealed record CreateProductCommand(
    string Name,
    string SKU,
    int CategoryTableId,
    string CategoryCode,
    decimal UnitPrice,
    int MinStock,
    int MaxStock) : ICommand<Result<ProductDto>>;
