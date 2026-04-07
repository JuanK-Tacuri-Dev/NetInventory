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
            var interval = await GetIntervalAsync(ct);
            await CheckLowStockAsync(ct);
            await Task.Delay(interval, ct);
        }
    }

    private async Task<TimeSpan> GetIntervalAsync(CancellationToken ct)
    {
        try
        {
            using var scope = scopeFactory.CreateScope();
            var generalValueRepo = scope.ServiceProvider.GetRequiredService<IGeneralValueRepository>();
            var values = await generalValueRepo.GetByTableIdAsync(1002, ct);
            var raw = values.FirstOrDefault(v => v.Code == "LOW_STOCK_MINUTES")?.Value;
            if (int.TryParse(raw, out var minutes) && minutes > 0)
                return TimeSpan.FromMinutes(minutes);
        }
        catch (Exception ex)
        {
            logger.LogWarning(ex, "No se pudo leer LOW_STOCK_HOURS desde la BD. Usando 1 hora por defecto.");
        }
        return TimeSpan.FromMinutes(60);
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
