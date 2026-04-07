namespace NetInventory.Api.Middleware;

public sealed class CorrelationIdMiddleware(RequestDelegate next)
{
    public async Task InvokeAsync(HttpContext context)
    {
        var correlationId = context.Request.Headers[Constants.Headers.CorrelationId].FirstOrDefault()
            ?? Guid.NewGuid().ToString();

        context.Items[Constants.Context.CorrelationId] = correlationId;
        context.Response.Headers[Constants.Headers.CorrelationId] = correlationId;

        await next(context);
    }
}
