using NetInventory.Domain.Entities;

namespace NetInventory.Domain.Interfaces;

public interface IProductRepository
{
    Task<Product?> GetByIdAsync(Guid id, CancellationToken ct = default);
    Task<IEnumerable<Product>> GetAllAsync(string? category = null, bool lowStockOnly = false, int threshold = 10, CancellationToken ct = default);
    Task<bool> ExistsBySkuAsync(string sku, Guid? excludeId = null, CancellationToken ct = default);
    Task AddAsync(Product product, CancellationToken ct = default);
    Task UpdateAsync(Product product, CancellationToken ct = default);
    Task DeleteAsync(Product product, CancellationToken ct = default);
}
