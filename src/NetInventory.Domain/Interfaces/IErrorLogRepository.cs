using NetInventory.Domain.Entities;

namespace NetInventory.Domain.Interfaces;

public interface IErrorLogRepository
{
    Task AddAsync(ErrorLog errorLog, CancellationToken ct = default);
    Task<IEnumerable<ErrorLog>> GetAllAsync(int page, int pageSize, CancellationToken ct = default);
    Task<int> CountAsync(CancellationToken ct = default);
}
