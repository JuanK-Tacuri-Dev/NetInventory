using NetInventory.Domain.Entities;

namespace NetInventory.Domain.Interfaces;

public interface IGeneralValueRepository
{
    Task<IEnumerable<GeneralValue>> GetByTableIdAsync(int tableId, CancellationToken ct = default);
}
