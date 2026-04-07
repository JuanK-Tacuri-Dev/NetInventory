using NetInventory.Application.Common;
using NetInventory.Application.Common.DTOs;
using NetInventory.Domain.Common;

namespace NetInventory.Application.AuditLogs.Queries.GetAuditLogs;

public sealed record GetAuditLogsQuery(int Page = 1, int PageSize = 50) : IQuery<Result<PagedResult<AuditLogDto>>>;
