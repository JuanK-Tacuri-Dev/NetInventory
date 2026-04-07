using NetInventory.Application.Common.DTOs;

namespace NetInventory.Application.Common.Interfaces;

public interface IAuditConfigCache
{
    Task<IEnumerable<AuditConfigDto>> GetActiveConfigsAsync();
    void Invalidate();
}
