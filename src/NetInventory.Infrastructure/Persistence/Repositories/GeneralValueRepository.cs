using Microsoft.EntityFrameworkCore;
using NetInventory.Domain.Entities;
using NetInventory.Domain.Interfaces;

namespace NetInventory.Infrastructure.Persistence.Repositories;

public sealed class GeneralValueRepository(AppDbContext context) : IGeneralValueRepository
{
    public async Task<IEnumerable<GeneralValue>> GetByTableIdAsync(int tableId, CancellationToken ct = default)
        => await context.GeneralValues
            .Where(x => x.TableId == tableId && x.IsActive)
            .OrderBy(x => x.SortOrder)
            .AsNoTracking()
            .ToListAsync(ct);
}
