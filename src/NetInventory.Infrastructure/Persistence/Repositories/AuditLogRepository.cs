using Microsoft.EntityFrameworkCore;
using NetInventory.Domain.Entities;
using NetInventory.Domain.Interfaces;

namespace NetInventory.Infrastructure.Persistence.Repositories;

public sealed class AuditLogRepository(AppDbContext context) : IAuditLogRepository
{
    public async Task AddAsync(AuditLog auditLog, CancellationToken ct = default)
        => await context.AuditLogs.AddAsync(auditLog, ct);

    public async Task<IEnumerable<AuditLog>> GetPagedAsync(int page, int pageSize, CancellationToken ct = default)
        => await context.AuditLogs
            .AsNoTracking()
            .OrderByDescending(x => x.OccurredAt)
            .Skip((page - 1) * pageSize)
            .Take(pageSize)
            .ToListAsync(ct);

    public async Task<int> CountAsync(CancellationToken ct = default)
        => await context.AuditLogs.CountAsync(ct);

    public async Task<AuditLog?> GetByCorrelationIdAsync(string correlationId, CancellationToken ct = default)
        => await context.AuditLogs
            .AsNoTracking()
            .FirstOrDefaultAsync(x => x.CorrelationId == correlationId, ct);
}
