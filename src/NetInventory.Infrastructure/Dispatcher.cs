using Microsoft.Extensions.DependencyInjection;
using NetInventory.Application.Common;

namespace NetInventory.Infrastructure;

public sealed class Dispatcher(IServiceProvider sp) : IDispatcher
{
    public Task<TResult> SendAsync<TResult>(ICommand<TResult> command, CancellationToken ct = default)
    {
        var handlerType = typeof(ICommandHandler<,>).MakeGenericType(command.GetType(), typeof(TResult));

        dynamic handler = sp.GetRequiredService(handlerType);

        return handler.HandleAsync((dynamic)command, ct);
    }

    public Task<TResult> QueryAsync<TResult>(IQuery<TResult> query, CancellationToken ct = default)
    {
        var handlerType = typeof(IQueryHandler<,>).MakeGenericType(query.GetType(), typeof(TResult));

        dynamic handler = sp.GetRequiredService(handlerType);

        return handler.HandleAsync((dynamic)query, ct);
    }
}
