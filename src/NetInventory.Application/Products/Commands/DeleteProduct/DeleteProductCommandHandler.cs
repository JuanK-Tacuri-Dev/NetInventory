using NetInventory.Application.Common;
using NetInventory.Application.Common.Interfaces;
using NetInventory.Domain.Common;
using NetInventory.Domain.Interfaces;

namespace NetInventory.Application.Products.Commands.DeleteProduct;

public sealed class DeleteProductCommandHandler(
    IProductRepository productRepository,
    IUnitOfWork unitOfWork,
    ValidationBehavior<DeleteProductCommand> validator)
{
    public async Task<Result> HandleAsync(DeleteProductCommand command, CancellationToken ct = default)
    {
        var validation = validator.Validate(command);
        if (validation.IsFailure)
            return validation;

        var product = await productRepository.GetByIdAsync(command.Id, ct);
        if (product is null)
            return Result.Failure(Error.ProductNotFound);

        await productRepository.DeleteAsync(product, ct);
        await unitOfWork.SaveChangesAsync(ct);

        return Result.Success();
    }
}
