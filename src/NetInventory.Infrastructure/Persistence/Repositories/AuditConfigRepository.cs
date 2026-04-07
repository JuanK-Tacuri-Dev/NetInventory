using Microsoft.EntityFrameworkCore;
using NetInventory.Domain.Entities;
using NetInventory.Domain.Interfaces;

namespace NetInventory.Infrastructure.Persistence.Repositories;

public sealed class AuditConfigRepository(AppDbContext context) : IAuditConfigRepository
{
    public async Task<IEnumerable<AuditConfig>> GetAllAsync(CancellationToken ct = default)
        => await context.AuditConfigs
            .AsNoTracking()
            .OrderBy(x => x.Method)
            .ThenBy(x => x.UrlPattern)
            .ToListAsync(ct);

    public async Task<AuditConfig?> GetByIdAsync(Guid id, CancellationToken ct = default)
        => await context.AuditConfigs.FirstOrDefaultAsync(x => x.Id == id, ct);

    public async Task AddAsync(AuditConfig config, CancellationToken ct = default)
        => await context.AuditConfigs.AddAsync(config, ct);

    public Task UpdateAsync(AuditConfig config, CancellationToken ct = default)
    {
        context.AuditConfigs.Update(config);
        return Task.CompletedTask;
    }

    public Task DeleteAsync(AuditConfig config, CancellationToken ct = default)
    {
        context.AuditConfigs.Remove(config);
        return Task.CompletedTask;
    }
}
