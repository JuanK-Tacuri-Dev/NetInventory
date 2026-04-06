namespace NetInventory.Application.ErrorLogs.Queries.GetErrorLogs;

public sealed record GetErrorLogsQuery(int Page = 1, int PageSize = 200);
