using NetInventory.Domain.Common;
using NetInventory.Domain.Entities;
using NetInventory.Domain.Enums;

namespace NetInventory.Application.Services;

public sealed class OutboundStrategy : IMovementStrategy
{
    public MovementType MovementType => MovementType.Outbound;

    public Result Apply(Product product, int quantity)
    {
        if (product.QuantityInStock - quantity < 0)
            return Result.Failure(Error.StockNegative);

        return product.ApplyMovement(quantity, MovementType.Outbound);
    }
}
