using FluentAssertions;
using Moq;
using NetInventory.Application.Products.Queries.GetProducts;
using NetInventory.Domain.Entities;
using NetInventory.Domain.Interfaces;

namespace NetInventory.UnitTests.Application;

public sealed class GetProductsQueryHandlerTests
{
    private readonly Mock<IProductRepository> _productRepo = new();

    private GetProductsQueryHandler CreateHandler() => new(_productRepo.Object);

    [Fact]
    public async Task HandleAsync_WithNoFilter_ReturnsAllProducts()
    {
        var products = new List<Product>
        {
            ApplicationTestHelpers.CreateProduct("SKU-001", 20),
            ApplicationTestHelpers.CreateProduct("SKU-002", 5)
        };
        _productRepo
            .Setup(r => r.GetAllAsync(null, false, 10, It.IsAny<CancellationToken>()))
            .ReturnsAsync(products);

        var query = new GetProductsQuery(null, false);
        var handler = CreateHandler();

        var result = await handler.HandleAsync(query);

        result.IsSuccess.Should().BeTrue();
        result.Value.Should().HaveCount(2);
    }

    [Fact]
    public async Task HandleAsync_WithCategoryFilter_ReturnsFilteredProducts()
    {
        var electronics = ApplicationTestHelpers.CreateProduct("SKU-001", 20);
        _productRepo
            .Setup(r => r.GetAllAsync("Electronics", false, 10, It.IsAny<CancellationToken>()))
            .ReturnsAsync(new List<Product> { electronics });

        var query = new GetProductsQuery("Electronics", false);
        var handler = CreateHandler();

        var result = await handler.HandleAsync(query);

        result.IsSuccess.Should().BeTrue();
        result.Value.Should().HaveCount(1);
        _productRepo.Verify(r => r.GetAllAsync("Electronics", false, 10, It.IsAny<CancellationToken>()), Times.Once);
    }

    [Fact]
    public async Task HandleAsync_WithLowStockFilter_ReturnsLowStockProducts()
    {
        var lowStock = ApplicationTestHelpers.CreateProduct("SKU-003", 3);
        _productRepo
            .Setup(r => r.GetAllAsync(null, true, 10, It.IsAny<CancellationToken>()))
            .ReturnsAsync(new List<Product> { lowStock });

        var query = new GetProductsQuery(null, true);
        var handler = CreateHandler();

        var result = await handler.HandleAsync(query);

        result.IsSuccess.Should().BeTrue();
        result.Value.Should().HaveCount(1);
        result.Value.First().IsLowStock.Should().BeTrue();
        _productRepo.Verify(r => r.GetAllAsync(null, true, 10, It.IsAny<CancellationToken>()), Times.Once);
    }
}
