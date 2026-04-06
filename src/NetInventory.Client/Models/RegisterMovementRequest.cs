using System.ComponentModel.DataAnnotations;

namespace NetInventory.Client.Models;

public class RegisterMovementRequest
{
    [Required]
    public string Type { get; set; } = "Inbound";

    [Range(1, int.MaxValue)]
    public int Quantity { get; set; } = 1;
}
