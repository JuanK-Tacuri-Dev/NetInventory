using NetInventory.Domain.Entities;

namespace NetInventory.Domain.Interfaces;

public interface IProductRepository
{
    Task<Product?> GetByIdAsync(Guid id, string ownerId, CancellationToken ct = default);
    Task<IEnumerable<Product>> GetAllAsync(string? ownerId = null, string? categoryCode = null, bool lowStockOnly = false, CancellationToken ct = default);
    Task<bool> ExistsBySkuAsync(string sku, string ownerId, Guid? excludeId = null, CancellationToken ct = default);
    Task AddAsync(Product product, CancellationToken ct = default);
    Task UpdateAsync(Product product, CancellationToken ct = default);
    Task DeleteAsync(Product product, CancellationToken ct = default);
}
