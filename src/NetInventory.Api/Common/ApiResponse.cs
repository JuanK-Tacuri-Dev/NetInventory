namespace NetInventory.Api.Common;

public sealed record ApiResponse<T>(bool Success, T? Data, string? Error, string? ErrorCode)
{
    public static ApiResponse<T> Ok(T data) => new(true, data, null, null);
    public static ApiResponse<T> Fail(string error, string errorCode) => new(false, default, error, errorCode);
}

public sealed record ApiResponse(bool Success, string? Error, string? ErrorCode)
{
    public static ApiResponse Ok() => new(true, null, null);
    public static ApiResponse Fail(string error, string errorCode) => new(false, error, errorCode);
}
