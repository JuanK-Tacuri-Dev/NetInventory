using FluentValidation;

namespace NetInventory.Application.Products.Commands.CreateProduct;

public sealed class CreateProductCommandValidator : AbstractValidator<CreateProductCommand>
{
    public CreateProductCommandValidator()
    {
        RuleFor(x => x.Name)
            .NotEmpty()
            .MaximumLength(200);

        RuleFor(x => x.SKU)
            .NotEmpty()
            .MaximumLength(50);

        RuleFor(x => x.Category)
            .NotEmpty();

        RuleFor(x => x.UnitPrice)
            .GreaterThanOrEqualTo(0);
    }
}
