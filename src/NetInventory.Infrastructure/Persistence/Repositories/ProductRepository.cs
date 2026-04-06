using Microsoft.EntityFrameworkCore;
using NetInventory.Domain.Entities;
using NetInventory.Domain.Interfaces;

namespace NetInventory.Infrastructure.Persistence.Repositories;

public sealed class ProductRepository(AppDbContext context) : IProductRepository
{
    public async Task<Product?> GetByIdAsync(Guid id, CancellationToken ct = default)
        => await context.Products.FirstOrDefaultAsync(x => x.Id == id, ct);

    public async Task<IEnumerable<Product>> GetAllAsync(
        string? category = null,
        bool lowStockOnly = false,
        int threshold = 10,
        CancellationToken ct = default)
        => await context.Products
            .Where(x => category == null || x.Category == category)
            .Where(x => !lowStockOnly || x.QuantityInStock < threshold)
            .AsNoTracking()
            .ToListAsync(ct);

    public async Task<bool> ExistsBySkuAsync(string sku, Guid? excludeId = null, CancellationToken ct = default)
        => await context.Products
            .AnyAsync(x => x.SKU.Value == sku && (excludeId == null || x.Id != excludeId), ct);

    public async Task AddAsync(Product product, CancellationToken ct = default)
        => await context.Products.AddAsync(product, ct);

    public Task UpdateAsync(Product product, CancellationToken ct = default)
    {
        context.Products.Update(product);
        return Task.CompletedTask;
    }

    public Task DeleteAsync(Product product, CancellationToken ct = default)
    {
        context.Products.Remove(product);
        return Task.CompletedTask;
    }
}
