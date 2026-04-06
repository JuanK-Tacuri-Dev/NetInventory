using Microsoft.EntityFrameworkCore;
using NetInventory.Domain.Entities;
using NetInventory.Domain.Interfaces;

namespace NetInventory.Infrastructure.Persistence.Repositories;

public sealed class ErrorLogRepository(AppDbContext context) : IErrorLogRepository
{
    public async Task AddAsync(ErrorLog errorLog, CancellationToken ct = default)
    {
        context.ErrorLogs.Add(errorLog);
        await context.SaveChangesAsync(ct);
    }

    public async Task<IEnumerable<ErrorLog>> GetAllAsync(int page, int pageSize, CancellationToken ct = default)
        => await context.ErrorLogs
            .AsNoTracking()
            .OrderByDescending(x => x.OccurredAt)
            .Skip((page - 1) * pageSize)
            .Take(pageSize)
            .ToListAsync(ct);

    public async Task<int> CountAsync(CancellationToken ct = default)
        => await context.ErrorLogs.CountAsync(ct);
}
