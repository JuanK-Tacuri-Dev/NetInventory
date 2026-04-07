using NetInventory.Domain.Entities;

namespace NetInventory.Domain.Interfaces;

public interface IAuditConfigRepository
{
    Task<IEnumerable<AuditConfig>> GetAllAsync(CancellationToken ct = default);
    Task<AuditConfig?> GetByIdAsync(Guid id, CancellationToken ct = default);
    Task AddAsync(AuditConfig config, CancellationToken ct = default);
    Task UpdateAsync(AuditConfig config, CancellationToken ct = default);
    Task DeleteAsync(AuditConfig config, CancellationToken ct = default);
}
