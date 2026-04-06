using NetInventory.Domain.Common;
using NetInventory.Domain.Entities;
using NetInventory.Domain.Enums;

namespace NetInventory.Application.Services;

public interface IMovementStrategy
{
    MovementType MovementType { get; }
    Result Apply(Product product, int quantity);
}
