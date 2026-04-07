using Mapster;
using NetInventory.Application.Common;
using NetInventory.Application.Common.DTOs;
using NetInventory.Domain.Interfaces;

namespace NetInventory.Application.ErrorLogs.Queries.GetErrorLogs;

public sealed class GetErrorLogsQueryHandler(IErrorLogRepository repository)
    : IQueryHandler<GetErrorLogsQuery, PagedResult<ErrorLogDto>>
{
    public async Task<PagedResult<ErrorLogDto>> HandleAsync(GetErrorLogsQuery query, CancellationToken ct = default)
    {
        var total = await repository.CountAsync(ct);
        var items = await repository.GetAllAsync(query.Page, query.PageSize, ct);

        var dtos = items.Adapt<IEnumerable<ErrorLogDto>>();

        return new PagedResult<ErrorLogDto>(dtos, total, query.Page, query.PageSize);
    }
}
