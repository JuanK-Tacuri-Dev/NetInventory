using Microsoft.EntityFrameworkCore;
using NetInventory.Domain.Entities;
using NetInventory.Domain.Interfaces;

namespace NetInventory.Infrastructure.Persistence.Repositories;

public sealed class GeneralTableRepository(AppDbContext context) : IGeneralTableRepository
{
    public async Task<IEnumerable<GeneralTable>> GetAllAsync(CancellationToken ct = default)
        => await context.GeneralTables
            .Where(x => x.IsActive)
            .AsNoTracking()
            .ToListAsync(ct);
}
