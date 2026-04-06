using NetInventory.Application.Common.DTOs;
using NetInventory.Domain.Interfaces;

namespace NetInventory.Application.ErrorLogs.Queries.GetErrorLogs;

public sealed class GetErrorLogsQueryHandler(IErrorLogRepository repository)
{
    public async Task<PagedResult<ErrorLogDto>> HandleAsync(GetErrorLogsQuery query, CancellationToken ct = default)
    {
        var total = await repository.CountAsync(ct);
        var items = await repository.GetAllAsync(query.Page, query.PageSize, ct);

        var dtos = items.Select(x => new ErrorLogDto(
            x.Id, x.CorrelationId, x.ExceptionType,
            x.Message, x.StackTrace, x.Path, x.Method, x.OccurredAt));

        return new PagedResult<ErrorLogDto>(dtos, total, query.Page, query.PageSize);
    }
}
