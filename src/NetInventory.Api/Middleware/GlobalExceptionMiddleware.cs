using System.Text.Json;
using NetInventory.Api.Common;
using NetInventory.Domain.Common;
using NetInventory.Domain.Entities;
using NetInventory.Domain.Interfaces;

namespace NetInventory.Api.Middleware;

public sealed class GlobalExceptionMiddleware(
    RequestDelegate next,
    ILogger<GlobalExceptionMiddleware> logger,
    IServiceScopeFactory scopeFactory)
{
    private static readonly JsonSerializerOptions JsonOptions = new() { PropertyNamingPolicy = JsonNamingPolicy.CamelCase };

    public async Task InvokeAsync(HttpContext context)
    {
        try
        {
            await next(context);
        }
        catch (Exception ex)
        {
            var correlationId = context.Items["CorrelationId"]?.ToString() ?? "N/A";
            logger.LogError(ex, "Unhandled exception. CorrelationId: {CorrelationId}", correlationId);

            try
            {
                using var scope = scopeFactory.CreateScope();
                var repo = scope.ServiceProvider.GetRequiredService<IErrorLogRepository>();
                var uow  = scope.ServiceProvider.GetRequiredService<IUnitOfWork>();
                await repo.AddAsync(ErrorLog.Create(
                    correlationId, ex,
                    context.Request.Path.ToString(),
                    context.Request.Method));
                await uow.SaveChangesAsync();
            }
            catch (Exception logEx)
            {
                logger.LogWarning(logEx, "No se pudo guardar el error log en BD.");
            }

            context.Response.StatusCode = StatusCodes.Status500InternalServerError;
            context.Response.ContentType = "application/json";
            var response = ApiResponse.Fail(Error.General.InternalError);
            await context.Response.WriteAsync(JsonSerializer.Serialize(response, JsonOptions));
        }
    }
}
