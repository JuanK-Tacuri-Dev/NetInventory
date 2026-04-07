using Mapster;
using NetInventory.Application.Common;
using NetInventory.Application.Common.DTOs;
using NetInventory.Application.Common.Interfaces;
using NetInventory.Domain.Common;
using NetInventory.Domain.Entities;
using NetInventory.Domain.Interfaces;
using NetInventory.Domain.ValueObjects;

namespace NetInventory.Application.Products.Commands.CreateProduct;

public sealed class CreateProductCommandHandler(
    IProductRepository productRepository,
    IUnitOfWork unitOfWork,
    ICurrentUserService currentUserService,
    ValidationBehavior<CreateProductCommand> validator)
    : ICommandHandler<CreateProductCommand, Result<ProductDto>>
{
    public async Task<Result<ProductDto>> HandleAsync(CreateProductCommand command, CancellationToken ct = default)
    {
        var validation = validator.Validate(command);
        if (validation.IsFailure)
            return Result.Failure<ProductDto>(validation.Error);

        var skuResult = Sku.Create(command.SKU);
        if (skuResult.IsFailure)
            return Result.Failure<ProductDto>(skuResult.Error);

        var sku = skuResult.Value;

        var ownerId = currentUserService.GetCurrentUserId();
        var exists = await productRepository.ExistsBySkuAsync(sku.Value, ownerId, null, ct);
        if (exists)
            return Result.Failure<ProductDto>(Error.Product.SkuDuplicated);

        var moneyResult = Money.Create(command.UnitPrice);
        if (moneyResult.IsFailure)
            return Result.Failure<ProductDto>(moneyResult.Error);

        var currentUser = currentUserService.GetCurrentUser();
        var product = Product.Create(command.Name, sku, command.CategoryTableId, command.CategoryCode, moneyResult.Value, command.MinStock, command.MaxStock, currentUser, ownerId);

        await productRepository.AddAsync(product, ct);
        await unitOfWork.SaveChangesAsync(ct);

        return Result.Success(product.Adapt<ProductDto>());
    }
}
