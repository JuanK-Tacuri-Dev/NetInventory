namespace NetInventory.Domain.Entities;

public sealed class GeneralValue
{
    public int Id { get; private set; }
    public int TableId { get; private set; }
    public string Code { get; private set; } = string.Empty;
    public string Description { get; private set; } = string.Empty;
    public int? ParentId { get; private set; }
    public int SortOrder { get; private set; }
    public string? Value { get; private set; }
    public bool IsActive { get; private set; }

    private GeneralValue() { }

    public static GeneralValue Create(int tableId, string code, string description, int? parentId = null, int sortOrder = 0, string? value = null) => new()
    {
        TableId = tableId,
        Code = code,
        Description = description,
        ParentId = parentId,
        SortOrder = sortOrder,
        Value = value,
        IsActive = true
    };
}
