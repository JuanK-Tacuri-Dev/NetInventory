namespace NetInventory.Domain.Entities;

public sealed class GeneralTable
{
    public int Id { get; private set; }
    public string Description { get; private set; } = string.Empty;
    public bool IsActive { get; private set; }

    private GeneralTable() { }

    public static GeneralTable Create(int id, string description) => new()
    {
        Id = id,
        Description = description,
        IsActive = true
    };
}
