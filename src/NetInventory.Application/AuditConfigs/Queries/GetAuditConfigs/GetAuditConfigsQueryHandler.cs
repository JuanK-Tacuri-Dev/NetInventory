using Mapster;
using NetInventory.Application.Common;
using NetInventory.Application.Common.DTOs;
using NetInventory.Domain.Common;
using NetInventory.Domain.Interfaces;

namespace NetInventory.Application.AuditConfigs.Queries.GetAuditConfigs;

public sealed class GetAuditConfigsQueryHandler(IAuditConfigRepository repository)
    : IQueryHandler<GetAuditConfigsQuery, Result<IEnumerable<AuditConfigDto>>>
{
    public async Task<Result<IEnumerable<AuditConfigDto>>> HandleAsync(
        GetAuditConfigsQuery query, CancellationToken ct = default)
    {
        var configs = await repository.GetAllAsync(ct);
        return Result.Success(configs.Adapt<IEnumerable<AuditConfigDto>>());
    }
}
