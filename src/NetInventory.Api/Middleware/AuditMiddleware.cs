using System.Diagnostics;
using System.Security.Claims;
using NetInventory.Application.Common.Interfaces;
using NetInventory.Domain.Entities;
using NetInventory.Domain.Interfaces;

namespace NetInventory.Api.Middleware;

public sealed class AuditMiddleware(
    RequestDelegate next,
    ILogger<AuditMiddleware> logger,
    IAuditConfigCache configCache,
    IServiceScopeFactory scopeFactory)
{
    public async Task InvokeAsync(HttpContext context)
    {
        var configs = await configCache.GetActiveConfigsAsync();
        var method = context.Request.Method;
        var path = context.Request.Path.ToString();

        if (!ShouldAudit(configs, method, path))
        {
            await next(context);
            return;
        }

        var correlationId = context.Items[Constants.Context.CorrelationId]?.ToString() ?? Guid.NewGuid().ToString("N")[..12];
        var sw = Stopwatch.StartNew();

        string? requestBody = null;
        var hasBody = context.Request.ContentLength is > 0
                   || (context.Request.ContentLength == null && context.Request.ContentType is not null);
        if (hasBody && !HttpMethods.IsGet(method) && !HttpMethods.IsHead(method))
        {
            context.Request.EnableBuffering();
            using var reader = new StreamReader(context.Request.Body, leaveOpen: true);
            var body = await reader.ReadToEndAsync();
            requestBody = body.Length > Constants.Audit.MaxBodyLength ? body[..Constants.Audit.MaxBodyLength] : body;
            context.Request.Body.Position = 0;
        }

        var originalBody = context.Response.Body;
        await using var ms = new MemoryStream();
        context.Response.Body = ms;

        try
        {
            await next(context);
        }
        finally
        {
            sw.Stop();

            ms.Position = 0;
            using var reader = new StreamReader(ms, leaveOpen: true);
            var rawResponse = await reader.ReadToEndAsync();
            var responseBody = rawResponse.Length > Constants.Audit.MaxBodyLength ? rawResponse[..Constants.Audit.MaxBodyLength] : rawResponse;

            ms.Position = 0;
            await ms.CopyToAsync(originalBody);
            context.Response.Body = originalBody;

            var userId = context.User.FindFirstValue(ClaimTypes.NameIdentifier);
            var userEmail = context.User.FindFirstValue(ClaimTypes.Name);
            var queryString = context.Request.QueryString.HasValue ? context.Request.QueryString.ToString() : null;

            try
            {
                using var scope = scopeFactory.CreateScope();
                var repo = scope.ServiceProvider.GetRequiredService<IAuditLogRepository>();
                var uow = scope.ServiceProvider.GetRequiredService<IUnitOfWork>();

                await repo.AddAsync(AuditLog.Create(
                    correlationId, method, path, queryString,
                    requestBody, responseBody,
                    context.Response.StatusCode, sw.ElapsedMilliseconds,
                    userId, userEmail));

                await uow.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                logger.LogWarning(ex, "No se pudo guardar el audit log. CorrelationId: {CorrelationId}", correlationId);
            }
        }
    }

    private static bool ShouldAudit(IEnumerable<Application.Common.DTOs.AuditConfigDto> configs, string method, string path)
        => configs.Any(c => c.IsEnabled
            && (c.Method == "*" || c.Method.Equals(method, StringComparison.OrdinalIgnoreCase))
            && path.StartsWith(c.UrlPattern, StringComparison.OrdinalIgnoreCase));
}
