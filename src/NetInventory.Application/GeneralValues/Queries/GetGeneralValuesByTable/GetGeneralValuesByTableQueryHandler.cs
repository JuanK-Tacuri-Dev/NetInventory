using Mapster;
using NetInventory.Application.Common;
using NetInventory.Application.Common.DTOs;
using NetInventory.Application.Common.Interfaces;
using NetInventory.Domain.Common;
using NetInventory.Domain.Interfaces;

namespace NetInventory.Application.GeneralValues.Queries.GetGeneralValuesByTable;

public sealed class GetGeneralValuesByTableQueryHandler(
    IGeneralValueRepository repository,
    ICacheService cache,
    CacheSettings cacheSettings)
    : IQueryHandler<GetGeneralValuesByTableQuery, Result<IEnumerable<GeneralValueDto>>>
{
    public async Task<Result<IEnumerable<GeneralValueDto>>> HandleAsync(
        GetGeneralValuesByTableQuery query, CancellationToken ct = default)
    {
        var expiration = TimeSpan.FromMinutes(cacheSettings.GeneralValuesMinutes);
        var key = $"{Constants.Cache.GeneralValuesPrefix}{query.TableId}";

        var values = await cache.GetOrSetAsync(
            key,
            async () =>
            {
                var data = await repository.GetByTableIdAsync(query.TableId, ct);
                return data.Adapt<IEnumerable<GeneralValueDto>>();
            },
            expiration);

        return Result.Success(values ?? []);
    }
}
