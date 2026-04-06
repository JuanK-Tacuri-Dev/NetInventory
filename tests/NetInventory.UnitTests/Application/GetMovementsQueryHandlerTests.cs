using FluentAssertions;
using Moq;
using NetInventory.Application.StockMovements.Queries.GetMovements;
using NetInventory.Domain.Common;
using NetInventory.Domain.Entities;
using NetInventory.Domain.Enums;
using NetInventory.Domain.Interfaces;

namespace NetInventory.UnitTests.Application;

public sealed class GetMovementsQueryHandlerTests
{
    private readonly Mock<IProductRepository> _productRepo = new();
    private readonly Mock<IStockMovementRepository> _movementRepo = new();

    private GetMovementsQueryHandler CreateHandler() =>
        new(_productRepo.Object, _movementRepo.Object);

    [Fact]
    public async Task HandleAsync_WithExistingProduct_ReturnsMovements()
    {
        var product = ApplicationTestHelpers.CreateProduct("SKU-001", 50);
        var movement1 = StockMovement.Create(product.Id, MovementType.Inbound, 50, "user1");
        var movement2 = StockMovement.Create(product.Id, MovementType.Outbound, 10, "user2");

        _productRepo.Setup(r => r.GetByIdAsync(product.Id, It.IsAny<CancellationToken>())).ReturnsAsync(product);
        _movementRepo
            .Setup(r => r.GetByProductIdAsync(product.Id, It.IsAny<CancellationToken>()))
            .ReturnsAsync(new List<StockMovement> { movement1, movement2 });

        var query = new GetMovementsQuery(product.Id);
        var handler = CreateHandler();

        var result = await handler.HandleAsync(query);

        result.IsSuccess.Should().BeTrue();
        result.Value.Should().HaveCount(2);
        result.Value.First().ProductId.Should().Be(product.Id);
        result.Value.First().Type.Should().Be("Inbound");
    }

    [Fact]
    public async Task HandleAsync_WithNonExistentProduct_ReturnsProductNotFoundError()
    {
        var id = Guid.NewGuid();
        _productRepo.Setup(r => r.GetByIdAsync(id, It.IsAny<CancellationToken>())).ReturnsAsync((Product?)null);

        var query = new GetMovementsQuery(id);
        var handler = CreateHandler();

        var result = await handler.HandleAsync(query);

        result.IsFailure.Should().BeTrue();
        result.Error.Should().Be(Error.ProductNotFound);
        _movementRepo.Verify(r => r.GetByProductIdAsync(It.IsAny<Guid>(), It.IsAny<CancellationToken>()), Times.Never);
    }
}
