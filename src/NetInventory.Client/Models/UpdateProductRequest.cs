using System.ComponentModel.DataAnnotations;
using NetInventory.Client.Models.Validation;

namespace NetInventory.Client.Models;

public class UpdateProductRequest : IValidatableObject
{
    [Required(ErrorMessage = "El nombre es obligatorio.")]
    [MinLength(2, ErrorMessage = "El nombre debe tener al menos 2 caracteres.")]
    [MaxLength(200, ErrorMessage = "El nombre no puede superar 200 caracteres.")]
    [NoWhitespace(ErrorMessage = "El nombre no puede contener solo espacios.")]
    [SafeText(ErrorMessage = "El nombre contiene caracteres no permitidos (< > ; \" ' &).")]
    public string Name { get; set; } = string.Empty;

    [Range(1, int.MaxValue, ErrorMessage = "La categoría es obligatoria.")]
    public int CategoryTableId { get; set; }

    [Required(ErrorMessage = "La categoría es obligatoria.")]
    public string CategoryCode { get; set; } = string.Empty;

    [Range(0.01, 9_999_999.99, ErrorMessage = "El precio debe ser mayor a cero y no superar $9,999,999.99.")]
    [MaxDecimalPlaces(2, ErrorMessage = "El precio no puede tener más de 2 decimales.")]
    public decimal UnitPrice { get; set; }

    [Range(0, 999_999, ErrorMessage = "El stock mínimo debe estar entre 0 y 999,999.")]
    public int MinStock { get; set; }

    [Range(0, 999_999, ErrorMessage = "El stock máximo debe estar entre 0 y 999,999.")]
    public int MaxStock { get; set; }

    public IEnumerable<ValidationResult> Validate(ValidationContext ctx)
    {
        if (MaxStock > 0 && MinStock > 0 && MaxStock < MinStock)
            yield return new ValidationResult(
                "El stock máximo debe ser mayor o igual al stock mínimo.",
                [nameof(MaxStock)]);
    }
}
