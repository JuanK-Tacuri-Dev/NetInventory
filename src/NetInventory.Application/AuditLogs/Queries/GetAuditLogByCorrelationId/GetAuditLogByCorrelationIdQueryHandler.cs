using Mapster;
using NetInventory.Application.Common;
using NetInventory.Application.Common.DTOs;
using NetInventory.Domain.Common;
using NetInventory.Domain.Interfaces;

namespace NetInventory.Application.AuditLogs.Queries.GetAuditLogByCorrelationId;

public sealed class GetAuditLogByCorrelationIdQueryHandler(IAuditLogRepository repository)
    : IQueryHandler<GetAuditLogByCorrelationIdQuery, Result<AuditLogDto>>
{
    public async Task<Result<AuditLogDto>> HandleAsync(
        GetAuditLogByCorrelationIdQuery query, CancellationToken ct = default)
    {
        var log = await repository.GetByCorrelationIdAsync(query.CorrelationId, ct);
        if (log is null)
            return Result.Failure<AuditLogDto>(Error.General.NotFound);

        return Result.Success(log.Adapt<AuditLogDto>());
    }
}
