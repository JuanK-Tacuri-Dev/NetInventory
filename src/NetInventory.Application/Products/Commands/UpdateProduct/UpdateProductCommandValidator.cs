using FluentValidation;
using NetInventory.Resources;

namespace NetInventory.Application.Products.Commands.UpdateProduct;

public sealed class UpdateProductCommandValidator : AbstractValidator<UpdateProductCommand>
{
    private static readonly char[] ForbiddenChars = ['<', '>', ';', '"', '\'', '&'];

    public UpdateProductCommandValidator()
    {
        RuleFor(x => x.Id)
            .NotEmpty().WithMessage(Messages.Val_Product_IdRequired);

        RuleFor(x => x.Name)
            .NotEmpty().WithMessage(Messages.Val_Product_NameRequired)
            .Must(n => !string.IsNullOrWhiteSpace(n)).WithMessage("El nombre no puede contener solo espacios.")
            .MinimumLength(2).WithMessage("El nombre debe tener al menos 2 caracteres.")
            .MaximumLength(200).WithMessage(Messages.Val_Product_NameMaxLength)
            .Must(n => n.IndexOfAny(ForbiddenChars) < 0).WithMessage("El nombre contiene caracteres no permitidos (< > ; \" ' &).");

        RuleFor(x => x.CategoryTableId)
            .GreaterThan(0).WithMessage(Messages.Val_Product_CategoryTableRequired);

        RuleFor(x => x.CategoryCode)
            .NotEmpty().WithMessage(Messages.Val_Product_CategoryRequired)
            .MaximumLength(20).WithMessage(Messages.Val_Product_CategoryCodeMaxLength);

        RuleFor(x => x.UnitPrice)
            .GreaterThan(0).WithMessage("El precio debe ser mayor a cero.")
            .LessThanOrEqualTo(9_999_999.99m).WithMessage("El precio no puede superar $9,999,999.99.")
            .Must(p => decimal.Round(p, 2) == p).WithMessage("El precio no puede tener más de 2 decimales.");

        RuleFor(x => x.MinStock)
            .GreaterThanOrEqualTo(0).WithMessage("El stock mínimo no puede ser negativo.")
            .LessThanOrEqualTo(999_999).WithMessage("El stock mínimo no puede superar 999,999.");

        RuleFor(x => x.MaxStock)
            .GreaterThanOrEqualTo(0).WithMessage("El stock máximo no puede ser negativo.")
            .LessThanOrEqualTo(999_999).WithMessage("El stock máximo no puede superar 999,999.")
            .Must((cmd, max) => max == 0 || cmd.MinStock == 0 || max >= cmd.MinStock)
            .WithMessage("El stock máximo debe ser mayor o igual al stock mínimo.");
    }
}
