using System.ComponentModel.DataAnnotations;
using NetInventory.Client.Models.Validation;

namespace NetInventory.Client.Models;

public class AuditConfigModel
{
    public Guid Id { get; set; }

    [Required(ErrorMessage = "El método es obligatorio.")]
    public string Method { get; set; } = string.Empty;

    [Required(ErrorMessage = "El patrón de URL es obligatorio.")]
    [MaxLength(500, ErrorMessage = "El patrón no puede superar 500 caracteres.")]
    [RegularExpression(@"^/.*", ErrorMessage = "El patrón de URL debe comenzar con /.")]
    public string UrlPattern { get; set; } = string.Empty;

    [Required(ErrorMessage = "La descripción es obligatoria.")]
    [MinLength(3, ErrorMessage = "La descripción debe tener al menos 3 caracteres.")]
    [MaxLength(200, ErrorMessage = "La descripción no puede superar 200 caracteres.")]
    [NoWhitespace(ErrorMessage = "La descripción no puede contener solo espacios.")]
    public string Description { get; set; } = string.Empty;

    public bool IsEnabled { get; set; }
    public DateTime CreatedAt { get; set; }
}
