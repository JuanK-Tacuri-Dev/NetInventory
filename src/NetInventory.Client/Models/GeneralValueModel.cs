namespace NetInventory.Client.Models;

public class GeneralValueModel
{
    public int Id { get; set; }
    public int TableId { get; set; }
    public string Code { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public int? ParentId { get; set; }
    public int SortOrder { get; set; }
    public string? Value { get; set; }
}
