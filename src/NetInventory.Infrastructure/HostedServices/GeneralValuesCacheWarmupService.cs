using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using NetInventory.Application.Common;
using NetInventory.Application.GeneralValues.Queries.GetGeneralValuesByTable;

namespace NetInventory.Infrastructure.HostedServices;

public sealed class GeneralValuesCacheWarmupService(IServiceProvider serviceProvider) : IHostedService
{
    public async Task StartAsync(CancellationToken cancellationToken)
    {
        using var scope = serviceProvider.CreateScope();
        var dispatcher = scope.ServiceProvider.GetRequiredService<IDispatcher>();

        // Precarga las tablas de parámetros generales conocidas
        await dispatcher.QueryAsync(new GetGeneralValuesByTableQuery(NetInventory.Application.Constants.GeneralTables.CategoryTableId), cancellationToken);
    }

    public Task StopAsync(CancellationToken cancellationToken) => Task.CompletedTask;
}
