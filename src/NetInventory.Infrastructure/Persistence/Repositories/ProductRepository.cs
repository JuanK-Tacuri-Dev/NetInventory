using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using NetInventory.Domain.Entities;
using NetInventory.Domain.Interfaces;

namespace NetInventory.Infrastructure.Persistence.Repositories;

public sealed class ProductRepository(AppDbContext context, ILogger<ProductRepository> logger) : IProductRepository
{
    public async Task<Product?> GetByIdAsync(Guid id, string ownerId, CancellationToken ct = default)
    {
        logger.LogWarning("GetByIdAsync id={Id} ownerId={OwnerId}", id, ownerId);
        var product = await context.Products.FirstOrDefaultAsync(x => x.Id == id && x.OwnerId == ownerId, ct);
        logger.LogWarning("GetByIdAsync result={Found}", product is null ? "NULL" : product.Name);
        return product;
    }

    public async Task<IEnumerable<Product>> GetAllAsync(
        string? ownerId = null,
        string? categoryCode = null,
        bool lowStockOnly = false,
        CancellationToken ct = default)
        => await context.Products
            .Where(x => ownerId == null || x.OwnerId == ownerId)
            .Where(x => categoryCode == null || x.CategoryCode == categoryCode)
            .Where(x => !lowStockOnly || x.QuantityInStock < x.MinStock)
            .AsNoTracking()
            .ToListAsync(ct);

    public async Task<bool> ExistsBySkuAsync(string sku, string ownerId, Guid? excludeId = null, CancellationToken ct = default)
        => await context.Products
            .AnyAsync(x => x.SKU.Value == sku && x.OwnerId == ownerId && (excludeId == null || x.Id != excludeId), ct);

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
