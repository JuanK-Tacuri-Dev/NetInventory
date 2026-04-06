using NetInventory.Domain.Entities;

namespace NetInventory.Domain.Interfaces;

public interface IStockMovementRepository
{
    Task<IEnumerable<StockMovement>> GetByProductIdAsync(Guid productId, CancellationToken ct = default);
    Task AddAsync(StockMovement movement, CancellationToken ct = default);
}
