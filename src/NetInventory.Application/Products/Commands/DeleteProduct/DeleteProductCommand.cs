using NetInventory.Application.Common;
using NetInventory.Domain.Common;

namespace NetInventory.Application.Products.Commands.DeleteProduct;

public sealed record DeleteProductCommand(Guid Id) : ICommand<Result>;
