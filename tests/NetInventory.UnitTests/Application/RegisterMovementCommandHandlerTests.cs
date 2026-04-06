using FluentAssertions;
using FluentValidation;
using Microsoft.Extensions.DependencyInjection;
using Moq;
using NetInventory.Application.Common;
using NetInventory.Application.Common.Interfaces;
using NetInventory.Application.Services;
using NetInventory.Application.StockMovements.Commands.RegisterMovement;
using NetInventory.Domain.Common;
using NetInventory.Domain.Entities;
using NetInventory.Domain.Interfaces;

namespace NetInventory.UnitTests.Application;

public sealed class RegisterMovementCommandHandlerTests
{
    private readonly Mock<IProductRepository> _productRepo = new();
    private readonly Mock<IStockMovementRepository> _movementRepo = new();
    private readonly Mock<IUnitOfWork> _unitOfWork = new();
    private readonly Mock<ICurrentUserService> _currentUser = new();

    private RegisterMovementCommandHandler CreateHandler(
        IEnumerable<IValidator<RegisterMovementCommand>>? validators = null,
        IEnumerable<IMovementStrategy>? strategies = null)
    {
        validators ??= BuildValidators();
        strategies ??= new IMovementStrategy[] { new InboundStrategy(), new OutboundStrategy() };
        var behavior = new ValidationBehavior<RegisterMovementCommand>(validators);
        return new RegisterMovementCommandHandler(
            _productRepo.Object,
            _movementRepo.Object,
            _unitOfWork.Object,
            _currentUser.Object,
            behavior,
            strategies);
    }

    private static IEnumerable<IValidator<RegisterMovementCommand>> BuildValidators()
    {
        var services = new ServiceCollection();
        services.AddValidatorsFromAssembly(typeof(RegisterMovementCommand).Assembly);
        var provider = services.BuildServiceProvider();
        return provider.GetServices<IValidator<RegisterMovementCommand>>();
    }

    [Fact]
    public async Task HandleAsync_InboundMovement_IncreasesStockAndReturnsDto()
    {
        var product = ApplicationTestHelpers.CreateProduct("SKU-001", 0);
        _productRepo.Setup(r => r.GetByIdAsync(product.Id, It.IsAny<CancellationToken>())).ReturnsAsync(product);
        _productRepo.Setup(r => r.UpdateAsync(product, It.IsAny<CancellationToken>())).Returns(Task.CompletedTask);
        _movementRepo.Setup(r => r.AddAsync(It.IsAny<StockMovement>(), It.IsAny<CancellationToken>())).Returns(Task.CompletedTask);
        _unitOfWork.Setup(u => u.SaveChangesAsync(It.IsAny<CancellationToken>())).ReturnsAsync(1);
        _currentUser.Setup(s => s.GetCurrentUser()).Returns("warehouse-user");

        var command = new RegisterMovementCommand(product.Id, "Inbound", 50);
        var handler = CreateHandler();

        var result = await handler.HandleAsync(command);

        result.IsSuccess.Should().BeTrue();
        result.Value.Type.Should().Be("Inbound");
        result.Value.Quantity.Should().Be(50);
        product.QuantityInStock.Should().Be(50);
    }

    [Fact]
    public async Task HandleAsync_OutboundMovement_WithSufficientStock_ReturnsSuccess()
    {
        var product = ApplicationTestHelpers.CreateProduct("SKU-001", 100);
        _productRepo.Setup(r => r.GetByIdAsync(product.Id, It.IsAny<CancellationToken>())).ReturnsAsync(product);
        _productRepo.Setup(r => r.UpdateAsync(product, It.IsAny<CancellationToken>())).Returns(Task.CompletedTask);
        _movementRepo.Setup(r => r.AddAsync(It.IsAny<StockMovement>(), It.IsAny<CancellationToken>())).Returns(Task.CompletedTask);
        _unitOfWork.Setup(u => u.SaveChangesAsync(It.IsAny<CancellationToken>())).ReturnsAsync(1);
        _currentUser.Setup(s => s.GetCurrentUser()).Returns("warehouse-user");

        var command = new RegisterMovementCommand(product.Id, "Outbound", 30);
        var handler = CreateHandler();

        var result = await handler.HandleAsync(command);

        result.IsSuccess.Should().BeTrue();
        result.Value.Type.Should().Be("Outbound");
        product.QuantityInStock.Should().Be(70);
    }

    [Fact]
    public async Task HandleAsync_OutboundMovement_WithInsufficientStock_ReturnsStockNegativeError()
    {
        var product = ApplicationTestHelpers.CreateProduct("SKU-001", 5);
        _productRepo.Setup(r => r.GetByIdAsync(product.Id, It.IsAny<CancellationToken>())).ReturnsAsync(product);

        var command = new RegisterMovementCommand(product.Id, "Outbound", 100);
        var handler = CreateHandler();

        var result = await handler.HandleAsync(command);

        result.IsFailure.Should().BeTrue();
        result.Error.Should().Be(Error.StockNegative);
        _unitOfWork.Verify(u => u.SaveChangesAsync(It.IsAny<CancellationToken>()), Times.Never);
    }

    [Fact]
    public async Task HandleAsync_WithNonExistentProduct_ReturnsProductNotFoundError()
    {
        var id = Guid.NewGuid();
        _productRepo.Setup(r => r.GetByIdAsync(id, It.IsAny<CancellationToken>())).ReturnsAsync((Product?)null);

        var command = new RegisterMovementCommand(id, "Inbound", 10);
        var handler = CreateHandler();

        var result = await handler.HandleAsync(command);

        result.IsFailure.Should().BeTrue();
        result.Error.Should().Be(Error.ProductNotFound);
    }

    [Fact]
    public async Task HandleAsync_WithInvalidQuantity_ReturnsValidationError()
    {
        var command = new RegisterMovementCommand(Guid.NewGuid(), "Inbound", 0);
        var handler = CreateHandler();

        var result = await handler.HandleAsync(command);

        result.IsFailure.Should().BeTrue();
        result.Error.Code.Should().NotBeEmpty();
    }
}
