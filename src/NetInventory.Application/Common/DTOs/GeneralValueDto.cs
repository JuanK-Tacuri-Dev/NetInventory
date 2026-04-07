namespace NetInventory.Application.Common.DTOs;

public sealed record GeneralValueDto(int Id, int TableId, string Code, string Description, int? ParentId, int SortOrder, string? Value);
