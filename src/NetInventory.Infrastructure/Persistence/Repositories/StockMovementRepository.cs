using Microsoft.EntityFrameworkCore;
using NetInventory.Domain.Entities;
using NetInventory.Domain.Interfaces;

namespace NetInventory.Infrastructure.Persistence.Repositories;

public sealed class StockMovementRepository(AppDbContext context) : IStockMovementRepository
{
    public async Task<IEnumerable<StockMovement>> GetByProductIdAsync(Guid productId, CancellationToken ct = default)
        => await context.StockMovements
            .Where(m => m.ProductId == productId)
            .AsNoTracking()
            .ToListAsync(ct);

    public async Task AddAsync(StockMovement movement, CancellationToken ct = default)
        => await context.StockMovements.AddAsync(movement, ct);
}
