namespace NetInventory.Client.Models;

public record ApiResponse<T>(
    bool Success,
    T? Data,
    string? Error,
    string? ErrorCode);
