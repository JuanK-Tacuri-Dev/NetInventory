using FluentAssertions;
using NetInventory.Application.Services;
using NetInventory.Domain.Enums;

namespace NetInventory.UnitTests.Application;

public sealed class InboundStrategyTests
{
    private readonly InboundStrategy _strategy = new();

    [Fact]
    public void Apply_WithValidQuantity_ReturnsSuccess()
    {
        var product = ApplicationTestHelpers.CreateProduct("SKU-001", 0);

        var result = _strategy.Apply(product, 25);

        result.IsSuccess.Should().BeTrue();
    }

    [Fact]
    public void Apply_AlwaysAppliesInboundMovement()
    {
        var product = ApplicationTestHelpers.CreateProduct("SKU-001", 10);

        _strategy.Apply(product, 40);

        product.QuantityInStock.Should().Be(50);
        _strategy.MovementType.Should().Be(MovementType.Inbound);
    }
}
