using FluentAssertions;
using FluentValidation;
using Microsoft.Extensions.DependencyInjection;
using Moq;
using NetInventory.Application.Common;
using NetInventory.Application.Common.Interfaces;
using NetInventory.Application.Products.Commands.CreateProduct;
using NetInventory.Domain.Common;
using NetInventory.Domain.Interfaces;

namespace NetInventory.UnitTests.Application;

public sealed class CreateProductCommandHandlerTests
{
    private readonly Mock<IProductRepository> _productRepo = new();
    private readonly Mock<IUnitOfWork> _unitOfWork = new();
    private readonly Mock<ICurrentUserService> _currentUser = new();

    private CreateProductCommandHandler CreateHandler(IEnumerable<IValidator<CreateProductCommand>>? validators = null)
    {
        validators ??= BuildValidators();
        var behavior = new ValidationBehavior<CreateProductCommand>(validators);
        return new CreateProductCommandHandler(_productRepo.Object, _unitOfWork.Object, _currentUser.Object, behavior);
    }

    private static IEnumerable<IValidator<CreateProductCommand>> BuildValidators()
    {
        var services = new ServiceCollection();
        services.AddValidatorsFromAssembly(typeof(CreateProductCommand).Assembly);
        var provider = services.BuildServiceProvider();
        return provider.GetServices<IValidator<CreateProductCommand>>();
    }

    [Fact]
    public async Task HandleAsync_WithValidCommand_ReturnsSuccessWithProductDto()
    {
        _currentUser.Setup(s => s.GetCurrentUser()).Returns("test-user");
        _currentUser.Setup(s => s.GetCurrentUserId()).Returns("owner-1");
        _productRepo.Setup(r => r.ExistsBySkuAsync("SKU-001", It.IsAny<string>(), null, It.IsAny<CancellationToken>())).ReturnsAsync(false);
        _productRepo.Setup(r => r.AddAsync(It.IsAny<NetInventory.Domain.Entities.Product>(), It.IsAny<CancellationToken>())).Returns(Task.CompletedTask);
        _unitOfWork.Setup(u => u.SaveChangesAsync(It.IsAny<CancellationToken>())).ReturnsAsync(1);

        var command = new CreateProductCommand("Widget", "SKU-001", 1001, "001", 9.99m, 5, 100);
        var handler = CreateHandler();

        var result = await handler.HandleAsync(command);

        result.IsSuccess.Should().BeTrue();
        result.Value.SKU.Should().Be("SKU-001");
        result.Value.Name.Should().Be("Widget");
        result.Value.CategoryTableId.Should().Be(1001);
        result.Value.CategoryCode.Should().Be("001");
        result.Value.UnitPrice.Should().Be(9.99m);
    }

    [Fact]
    public async Task HandleAsync_WithDuplicateSku_ReturnsSkuDuplicatedError()
    {
        _currentUser.Setup(s => s.GetCurrentUserId()).Returns("owner-1");
        _productRepo.Setup(r => r.ExistsBySkuAsync("SKU-001", It.IsAny<string>(), null, It.IsAny<CancellationToken>())).ReturnsAsync(true);

        var command = new CreateProductCommand("Widget", "SKU-001", 1001, "001", 9.99m, 5, 100);
        var handler = CreateHandler();

        var result = await handler.HandleAsync(command);

        result.IsFailure.Should().BeTrue();
        result.Error.Should().Be(Error.Product.SkuDuplicated);
    }

    [Fact]
    public async Task HandleAsync_WithInvalidCommand_ReturnsValidationError()
    {
        var command = new CreateProductCommand("Widget", "", 1001, "001", 9.99m, 0, 0);
        var handler = CreateHandler();

        var result = await handler.HandleAsync(command);

        result.IsFailure.Should().BeTrue();
        result.Error.Code.Should().NotBeEmpty();
    }
}
