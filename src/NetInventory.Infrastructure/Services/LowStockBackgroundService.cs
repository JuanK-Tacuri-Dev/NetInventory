using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using NetInventory.Domain.Interfaces;

namespace NetInventory.Infrastructure.Services;

public sealed class LowStockBackgroundService(
    IServiceScopeFactory scopeFactory,
    ILogger<LowStockBackgroundService> logger) : BackgroundService
{
    protected override async Task ExecuteAsync(CancellationToken ct)
    {
        while (!ct.IsCancellationRequested)
        {
            await CheckLowStockAsync(ct);
            await Task.Delay(TimeSpan.FromHours(1), ct);
        }
    }

    private async Task CheckLowStockAsync(CancellationToken ct)
    {
        using var scope = scopeFactory.CreateScope();
        var repository = scope.ServiceProvider.GetRequiredService<IProductRepository>();
        var lowStock = (await repository.GetAllAsync(lowStockOnly: true, ct: ct)).ToList();

        if (lowStock.Count == 0) return;

        var skus = string.Join(", ", lowStock.Select(p => p.SKU.Value));
        logger.LogWarning("Productos con stock bajo ({Count}): {SKUs}", lowStock.Count, skus);
    }
}
