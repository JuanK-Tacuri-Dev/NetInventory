using FluentAssertions;
using NetInventory.Application.Services;
using NetInventory.Domain.Common;
using NetInventory.Domain.Enums;

namespace NetInventory.UnitTests.Application;

public sealed class OutboundStrategyTests
{
    private readonly OutboundStrategy _strategy = new();

    [Fact]
    public void Apply_WithSufficientStock_ReturnsSuccess()
    {
        var product = ApplicationTestHelpers.CreateProduct("SKU-001", 100);

        var result = _strategy.Apply(product, 30);

        result.IsSuccess.Should().BeTrue();
        product.QuantityInStock.Should().Be(70);
        _strategy.MovementType.Should().Be(MovementType.Outbound);
    }

    [Fact]
    public void Apply_WithInsufficientStock_ReturnsStockNegativeError()
    {
        var product = ApplicationTestHelpers.CreateProduct("SKU-001", 5);

        var result = _strategy.Apply(product, 10);

        result.IsFailure.Should().BeTrue();
        result.Error.Should().Be(Error.Stock.Negative);
        product.QuantityInStock.Should().Be(5);
    }
}
