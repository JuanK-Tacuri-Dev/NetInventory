using NetInventory.Application.Common;
using NetInventory.Application.Common.Interfaces;
using NetInventory.Domain.Common;
using NetInventory.Domain.Interfaces;
using NetInventory.Domain.ValueObjects;

namespace NetInventory.Application.Products.Commands.UpdateProduct;

public sealed class UpdateProductCommandHandler(
    IProductRepository productRepository,
    IUnitOfWork unitOfWork,
    ICurrentUserService currentUserService,
    ValidationBehavior<UpdateProductCommand> validator)
{
    public async Task<Result> HandleAsync(UpdateProductCommand command, CancellationToken ct = default)
    {
        var validation = validator.Validate(command);
        if (validation.IsFailure)
            return validation;

        var product = await productRepository.GetByIdAsync(command.Id, ct);
        if (product is null)
            return Result.Failure(Error.ProductNotFound);

        var moneyResult = Money.Create(command.UnitPrice);
        if (moneyResult.IsFailure)
            return Result.Failure(moneyResult.Error);

        var currentUser = currentUserService.GetCurrentUser();
        product.Update(command.Name, command.Category, moneyResult.Value, currentUser);

        await productRepository.UpdateAsync(product, ct);
        await unitOfWork.SaveChangesAsync(ct);

        return Result.Success();
    }
}
