using NetInventory.Domain.Entities;

namespace NetInventory.Domain.Interfaces;

public interface IGeneralTableRepository
{
    Task<IEnumerable<GeneralTable>> GetAllAsync(CancellationToken ct = default);
}
