using NetInventory.Application.Common;
using NetInventory.Application.Common.Interfaces;
using NetInventory.Domain.Common;
using NetInventory.Domain.Interfaces;

namespace NetInventory.Application.Products.Commands.DeleteProduct;

public sealed class DeleteProductCommandHandler(
    IProductRepository productRepository,
    IUnitOfWork unitOfWork,
    ICurrentUserService currentUserService,
    ValidationBehavior<DeleteProductCommand> validator)
    : ICommandHandler<DeleteProductCommand, Result>
{
    public async Task<Result> HandleAsync(DeleteProductCommand command, CancellationToken ct = default)
    {
        var validation = validator.Validate(command);
        if (validation.IsFailure)
            return validation;

        var ownerId = currentUserService.GetCurrentUserId();
        var product = await productRepository.GetByIdAsync(command.Id, ownerId, ct);
        if (product is null)
            return Result.Failure(Error.Product.NotFound);

        await productRepository.DeleteAsync(product, ct);
        await unitOfWork.SaveChangesAsync(ct);

        return Result.Success();
    }
}
