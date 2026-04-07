using FluentAssertions;
using FluentValidation;
using Microsoft.Extensions.DependencyInjection;
using Moq;
using NetInventory.Application.Common;
using NetInventory.Application.Common.Interfaces;
using NetInventory.Application.Products.Commands.DeleteProduct;
using NetInventory.Domain.Common;
using NetInventory.Domain.Interfaces;

namespace NetInventory.UnitTests.Application;

public sealed class DeleteProductCommandHandlerTests
{
    private readonly Mock<IProductRepository> _productRepo = new();
    private readonly Mock<IUnitOfWork> _unitOfWork = new();
    private readonly Mock<ICurrentUserService> _currentUser = new();

    private DeleteProductCommandHandler CreateHandler(IEnumerable<IValidator<DeleteProductCommand>>? validators = null)
    {
        _currentUser.Setup(s => s.GetCurrentUserId()).Returns("owner-1");
        validators ??= BuildValidators();
        var behavior = new ValidationBehavior<DeleteProductCommand>(validators);
        return new DeleteProductCommandHandler(_productRepo.Object, _unitOfWork.Object, _currentUser.Object, behavior);
    }

    private static IEnumerable<IValidator<DeleteProductCommand>> BuildValidators()
    {
        var services = new ServiceCollection();
        services.AddValidatorsFromAssembly(typeof(DeleteProductCommand).Assembly);
        var provider = services.BuildServiceProvider();
        return provider.GetServices<IValidator<DeleteProductCommand>>();
    }

    [Fact]
    public async Task HandleAsync_WithExistingProduct_ReturnsSuccess()
    {
        var product = ApplicationTestHelpers.CreateProduct();
        _productRepo.Setup(r => r.GetByIdAsync(product.Id, It.IsAny<string>(), It.IsAny<CancellationToken>())).ReturnsAsync(product);
        _productRepo.Setup(r => r.DeleteAsync(product, It.IsAny<CancellationToken>())).Returns(Task.CompletedTask);
        _unitOfWork.Setup(u => u.SaveChangesAsync(It.IsAny<CancellationToken>())).ReturnsAsync(1);

        var command = new DeleteProductCommand(product.Id);
        var handler = CreateHandler();

        var result = await handler.HandleAsync(command);

        result.IsSuccess.Should().BeTrue();
        _productRepo.Verify(r => r.DeleteAsync(product, It.IsAny<CancellationToken>()), Times.Once);
        _unitOfWork.Verify(u => u.SaveChangesAsync(It.IsAny<CancellationToken>()), Times.Once);
    }

    [Fact]
    public async Task HandleAsync_WithNonExistentProduct_ReturnsProductNotFoundError()
    {
        var id = Guid.NewGuid();
        _productRepo.Setup(r => r.GetByIdAsync(id, It.IsAny<string>(), It.IsAny<CancellationToken>())).ReturnsAsync((NetInventory.Domain.Entities.Product?)null);

        var command = new DeleteProductCommand(id);
        var handler = CreateHandler();

        var result = await handler.HandleAsync(command);

        result.IsFailure.Should().BeTrue();
        result.Error.Should().Be(Error.Product.NotFound);
        _productRepo.Verify(r => r.DeleteAsync(It.IsAny<NetInventory.Domain.Entities.Product>(), It.IsAny<CancellationToken>()), Times.Never);
    }
}
