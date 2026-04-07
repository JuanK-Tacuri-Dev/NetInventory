using System.ComponentModel.DataAnnotations;

namespace NetInventory.Client.Models.Validation;

/// <summary>Rechaza strings que sean solo espacios en blanco.</summary>
public sealed class NoWhitespaceAttribute : ValidationAttribute
{
    protected override ValidationResult? IsValid(object? value, ValidationContext ctx)
    {
        if (value is string s && !string.IsNullOrEmpty(s) && string.IsNullOrWhiteSpace(s))
            return new ValidationResult(ErrorMessage ?? "Este campo no puede contener solo espacios.");
        return ValidationResult.Success;
    }
}

/// <summary>Valida que un decimal no tenga más de N decimales.</summary>
public sealed class MaxDecimalPlacesAttribute(int places) : ValidationAttribute
{
    protected override ValidationResult? IsValid(object? value, ValidationContext ctx)
    {
        if (value is decimal d && decimal.Round(d, places) != d)
            return new ValidationResult(ErrorMessage ?? $"No puede tener más de {places} decimales.");
        return ValidationResult.Success;
    }
}

/// <summary>Valida que solo contenga letras, números, espacios y los símbolos permitidos.
/// Bloquea caracteres de inyección: &lt; &gt; ; " ' &amp;</summary>
public sealed class SafeTextAttribute : ValidationAttribute
{
    private static readonly char[] Forbidden = ['<', '>', ';', '"', '\'', '&'];

    protected override ValidationResult? IsValid(object? value, ValidationContext ctx)
    {
        if (value is string s && s.IndexOfAny(Forbidden) >= 0)
            return new ValidationResult(ErrorMessage ?? "El campo contiene caracteres no permitidos (< > ; \" ' &).");
        return ValidationResult.Success;
    }
}

/// <summary>Valida que el SKU solo contenga letras, números, guiones y guiones bajos.</summary>
public sealed class SkuFormatAttribute : ValidationAttribute
{
    protected override ValidationResult? IsValid(object? value, ValidationContext ctx)
    {
        if (value is string s && !string.IsNullOrEmpty(s))
        {
            foreach (var c in s)
                if (!char.IsLetterOrDigit(c) && c != '-' && c != '_')
                    return new ValidationResult(ErrorMessage ?? "El SKU solo puede contener letras, números, guiones (-) y guiones bajos (_).");
        }
        return ValidationResult.Success;
    }
}
