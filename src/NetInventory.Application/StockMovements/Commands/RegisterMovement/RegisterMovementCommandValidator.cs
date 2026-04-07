using FluentValidation;
using NetInventory.Resources;

namespace NetInventory.Application.StockMovements.Commands.RegisterMovement;

public sealed class RegisterMovementCommandValidator : AbstractValidator<RegisterMovementCommand>
{
    private static readonly char[] ForbiddenChars = ['<', '>', ';', '"', '\'', '&'];

    public RegisterMovementCommandValidator()
    {
        RuleFor(x => x.ProductId)
            .NotEmpty().WithMessage(Messages.Val_Movement_ProductIdRequired);

        RuleFor(x => x.Type)
            .NotEmpty().WithMessage(Messages.Val_Movement_TypeRequired)
            .Must(t => t == "Inbound" || t == "Outbound")
            .WithMessage(Messages.Val_Movement_TypeInvalid);

        RuleFor(x => x.Quantity)
            .GreaterThan(0).WithMessage(Messages.Val_Movement_QuantityGreaterThanZero)
            .LessThanOrEqualTo(10_000).WithMessage("La cantidad no puede superar 10,000 unidades.");

        RuleFor(x => x.Reason)
            .Must(r => string.IsNullOrEmpty(r) || !string.IsNullOrWhiteSpace(r))
            .WithMessage("El motivo no puede contener solo espacios.")
            .MaximumLength(500).WithMessage("El motivo no puede superar 500 caracteres.")
            .Must(r => r == null || r.IndexOfAny(ForbiddenChars) < 0)
            .WithMessage("El motivo contiene caracteres no permitidos (< > ; \" ' &).")
            .When(x => x.Reason != null);
    }
}
