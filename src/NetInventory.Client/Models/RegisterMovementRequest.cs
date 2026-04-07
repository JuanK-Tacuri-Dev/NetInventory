using System.ComponentModel.DataAnnotations;
using NetInventory.Client.Models.Validation;

namespace NetInventory.Client.Models;

public class RegisterMovementRequest
{
    [Required(ErrorMessage = "El tipo de movimiento es obligatorio.")]
    public string Type { get; set; } = "Inbound";

    [Range(1, 10_000, ErrorMessage = "La cantidad debe ser mayor a cero y no superar 10,000 unidades.")]
    public int Quantity { get; set; } = 1;

    [MaxLength(500, ErrorMessage = "El motivo no puede superar 500 caracteres.")]
    [NoWhitespace(ErrorMessage = "El motivo no puede contener solo espacios.")]
    [SafeText(ErrorMessage = "El motivo contiene caracteres no permitidos (< > ; \" ' &).")]
    public string? Reason { get; set; }
}
