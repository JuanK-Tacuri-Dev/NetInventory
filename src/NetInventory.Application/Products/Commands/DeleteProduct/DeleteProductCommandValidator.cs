using FluentValidation;
using NetInventory.Resources;

namespace NetInventory.Application.Products.Commands.DeleteProduct;

public sealed class DeleteProductCommandValidator : AbstractValidator<DeleteProductCommand>
{
    public DeleteProductCommandValidator()
    {
        RuleFor(x => x.Id)
            .NotEmpty().WithMessage(Messages.Val_Product_IdRequired);
    }
}
