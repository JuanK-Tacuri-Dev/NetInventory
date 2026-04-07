using FluentAssertions;
using FluentValidation;
using Microsoft.Extensions.DependencyInjection;
using Moq;
using NetInventory.Application.Common;
using NetInventory.Application.Common.Interfaces;
using NetInventory.Application.Products.Commands.UpdateProduct;
using NetInventory.Domain.Common;
using NetInventory.Domain.Interfaces;

namespace NetInventory.UnitTests.Application;

public sealed class UpdateProductCommandHandlerTests
{
    private readonly Mock<IProductRepository> _productRepo = new();
    private readonly Mock<IUnitOfWork> _unitOfWork = new();
    private readonly Mock<ICurrentUserService> _currentUser = new();

    private UpdateProductCommandHandler CreateHandler(IEnumerable<IValidator<UpdateProductCommand>>? validators = null)
    {
        validators ??= BuildValidators();
        var behavior = new ValidationBehavior<UpdateProductCommand>(validators);
        return new UpdateProductCommandHandler(_productRepo.Object, _unitOfWork.Object, _currentUser.Object, behavior);
    }

    private static IEnumerable<IValidator<UpdateProductCommand>> BuildValidators()
    {
        var services = new ServiceCollection();
        services.AddValidatorsFromAssembly(typeof(UpdateProductCommand).Assembly);
        var provider = services.BuildServiceProvider();
        return provider.GetServices<IValidator<UpdateProductCommand>>();
    }

    [Fact]
    public async Task HandleAsync_WithValidCommand_ReturnsSuccess()
    {
        var product = ApplicationTestHelpers.CreateProduct();
        _currentUser.Setup(s => s.GetCurrentUserId()).Returns("owner-1");
        _productRepo.Setup(r => r.GetByIdAsync(product.Id, It.IsAny<string>(), It.IsAny<CancellationToken>())).ReturnsAsync(product);
        _productRepo.Setup(r => r.UpdateAsync(product, It.IsAny<CancellationToken>())).Returns(Task.CompletedTask);
        _unitOfWork.Setup(u => u.SaveChangesAsync(It.IsAny<CancellationToken>())).ReturnsAsync(1);
        _currentUser.Setup(s => s.GetCurrentUser()).Returns("editor");

        var command = new UpdateProductCommand(product.Id, "Updated Name", 1001, "002", 25m, 5, 100);
        var handler = CreateHandler();

        var result = await handler.HandleAsync(command);

        result.IsSuccess.Should().BeTrue();
        product.Name.Should().Be("Updated Name");
    }

    [Fact]
    public async Task HandleAsync_WithNonExistentProduct_ReturnsProductNotFoundError()
    {
        var id = Guid.NewGuid();
        _currentUser.Setup(s => s.GetCurrentUserId()).Returns("owner-1");
        _productRepo.Setup(r => r.GetByIdAsync(id, It.IsAny<string>(), It.IsAny<CancellationToken>())).ReturnsAsync((NetInventory.Domain.Entities.Product?)null);

        var command = new UpdateProductCommand(id, "Updated Name", 1001, "001", 25m, 0, 0);
        var handler = CreateHandler();

        var result = await handler.HandleAsync(command);

        result.IsFailure.Should().BeTrue();
        result.Error.Should().Be(Error.Product.NotFound);
    }

    [Fact]
    public async Task HandleAsync_WithInvalidCommand_ReturnsValidationError()
    {
        var command = new UpdateProductCommand(Guid.NewGuid(), "", 1001, "001", 25m, 0, 0);
        var handler = CreateHandler();

        var result = await handler.HandleAsync(command);

        result.IsFailure.Should().BeTrue();
        result.Error.Code.Should().NotBeEmpty();
    }
}
