using NetInventory.Domain.Common;
using NetInventory.Domain.Entities;
using NetInventory.Domain.Enums;

namespace NetInventory.Application.Services;

public sealed class InboundStrategy : IMovementStrategy
{
    public MovementType MovementType => MovementType.Inbound;

    public Result Apply(Product product, int quantity) =>
        product.ApplyMovement(quantity, MovementType.Inbound);
}
