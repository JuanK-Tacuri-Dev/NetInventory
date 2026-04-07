using System.ComponentModel.DataAnnotations;

namespace NetInventory.Client.Models;

public class RegisterRequest
{
    [Required(ErrorMessage = "El correo es obligatorio.")]
    [EmailAddress(ErrorMessage = "Ingresa un correo electrónico válido.")]
    [MaxLength(256, ErrorMessage = "El correo no puede superar 256 caracteres.")]
    public string Email { get; set; } = string.Empty;

    [Required(ErrorMessage = "La contraseña es obligatoria.")]
    [MinLength(8, ErrorMessage = "La contraseña debe tener al menos 8 caracteres.")]
    [MaxLength(100, ErrorMessage = "La contraseña no puede superar 100 caracteres.")]
    public string Password { get; set; } = string.Empty;
}
