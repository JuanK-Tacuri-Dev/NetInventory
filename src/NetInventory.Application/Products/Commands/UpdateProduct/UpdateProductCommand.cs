using NetInventory.Application.Common;
using NetInventory.Domain.Common;

namespace NetInventory.Application.Products.Commands.UpdateProduct;

public sealed record UpdateProductCommand(Guid Id, string Name, int CategoryTableId, string CategoryCode, decimal UnitPrice, int MinStock, int MaxStock) : ICommand<Result>;
