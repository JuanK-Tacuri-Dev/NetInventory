using FluentValidation;

namespace NetInventory.Application.StockMovements.Commands.RegisterMovement;

public sealed class RegisterMovementCommandValidator : AbstractValidator<RegisterMovementCommand>
{
    public RegisterMovementCommandValidator()
    {
        RuleFor(x => x.ProductId)
            .NotEmpty();

        RuleFor(x => x.Type)
            .NotEmpty()
            .Must(t => t == "Inbound" || t == "Outbound")
            .WithMessage("El tipo de movimiento debe ser 'Inbound' o 'Outbound'.");

        RuleFor(x => x.Quantity)
            .GreaterThan(0);
    }
}
