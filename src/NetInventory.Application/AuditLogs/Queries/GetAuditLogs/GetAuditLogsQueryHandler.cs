using Mapster;
using NetInventory.Application.Common;
using NetInventory.Application.Common.DTOs;
using NetInventory.Domain.Common;
using NetInventory.Domain.Interfaces;

namespace NetInventory.Application.AuditLogs.Queries.GetAuditLogs;

public sealed class GetAuditLogsQueryHandler(IAuditLogRepository repository)
    : IQueryHandler<GetAuditLogsQuery, Result<PagedResult<AuditLogDto>>>
{
    public async Task<Result<PagedResult<AuditLogDto>>> HandleAsync(
        GetAuditLogsQuery query, CancellationToken ct = default)
    {
        var total = await repository.CountAsync(ct);
        var items = await repository.GetPagedAsync(query.Page, query.PageSize, ct);
        var dtos = items.Adapt<IEnumerable<AuditLogDto>>();
        return Result.Success(new PagedResult<AuditLogDto>(dtos, total, query.Page, query.PageSize));
    }
}
