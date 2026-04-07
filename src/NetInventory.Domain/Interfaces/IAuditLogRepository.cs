using NetInventory.Domain.Entities;

namespace NetInventory.Domain.Interfaces;

public interface IAuditLogRepository
{
    Task AddAsync(AuditLog auditLog, CancellationToken ct = default);
    Task<IEnumerable<AuditLog>> GetPagedAsync(int page, int pageSize, CancellationToken ct = default);
    Task<int> CountAsync(CancellationToken ct = default);
    Task<AuditLog?> GetByCorrelationIdAsync(string correlationId, CancellationToken ct = default);
}
