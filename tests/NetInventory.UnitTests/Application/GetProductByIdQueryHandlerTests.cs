using FluentAssertions;
using Moq;
using NetInventory.Application.Common.Interfaces;
using NetInventory.Application.Products.Queries.GetProductById;
using NetInventory.Domain.Common;
using NetInventory.Domain.Entities;
using NetInventory.Domain.Interfaces;

namespace NetInventory.UnitTests.Application;

public sealed class GetProductByIdQueryHandlerTests
{
    private readonly Mock<IProductRepository> _productRepo = new();
    private readonly Mock<ICurrentUserService> _currentUser = new();

    private GetProductByIdQueryHandler CreateHandler()
    {
        _currentUser.Setup(s => s.GetCurrentUserId()).Returns("owner-1");
        return new(_productRepo.Object, _currentUser.Object);
    }

    [Fact]
    public async Task HandleAsync_WithExistingId_ReturnsProductDto()
    {
        var product = ApplicationTestHelpers.CreateProduct("SKU-001", 15);
        _productRepo.Setup(r => r.GetByIdAsync(product.Id, It.IsAny<string>(), It.IsAny<CancellationToken>())).ReturnsAsync(product);

        var query = new GetProductByIdQuery(product.Id);
        var handler = CreateHandler();

        var result = await handler.HandleAsync(query);

        result.IsSuccess.Should().BeTrue();
        result.Value.Id.Should().Be(product.Id);
        result.Value.SKU.Should().Be("SKU-001");
        result.Value.QuantityInStock.Should().Be(15);
    }

    [Fact]
    public async Task HandleAsync_WithNonExistentId_ReturnsProductNotFoundError()
    {
        var id = Guid.NewGuid();
        _productRepo.Setup(r => r.GetByIdAsync(id, It.IsAny<string>(), It.IsAny<CancellationToken>())).ReturnsAsync((Product?)null);

        var query = new GetProductByIdQuery(id);
        var handler = CreateHandler();

        var result = await handler.HandleAsync(query);

        result.IsFailure.Should().BeTrue();
        result.Error.Should().Be(Error.Product.NotFound);
    }
}
