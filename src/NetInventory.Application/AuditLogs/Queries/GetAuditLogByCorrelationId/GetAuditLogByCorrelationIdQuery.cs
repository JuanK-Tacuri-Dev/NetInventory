using NetInventory.Application.Common;
using NetInventory.Application.Common.DTOs;
using NetInventory.Domain.Common;

namespace NetInventory.Application.AuditLogs.Queries.GetAuditLogByCorrelationId;

public sealed record GetAuditLogByCorrelationIdQuery(string CorrelationId) : IQuery<Result<AuditLogDto>>;
