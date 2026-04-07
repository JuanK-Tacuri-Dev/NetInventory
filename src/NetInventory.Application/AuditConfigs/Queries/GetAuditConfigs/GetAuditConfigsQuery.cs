using NetInventory.Application.Common;
using NetInventory.Application.Common.DTOs;
using NetInventory.Domain.Common;

namespace NetInventory.Application.AuditConfigs.Queries.GetAuditConfigs;

public sealed record GetAuditConfigsQuery() : IQuery<Result<IEnumerable<AuditConfigDto>>>;
