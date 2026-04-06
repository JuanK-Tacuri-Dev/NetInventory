using System.ComponentModel.DataAnnotations;

namespace NetInventory.Client.Models;

public class UpdateProductRequest
{
    [Required]
    public string Name { get; set; } = string.Empty;

    [Required]
    public string Category { get; set; } = string.Empty;

    [Range(0.01, double.MaxValue)]
    public decimal UnitPrice { get; set; }
}
