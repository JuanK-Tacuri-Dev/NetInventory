using FluentAssertions;
using NetInventory.Domain.ValueObjects;

namespace NetInventory.UnitTests.Domain;

public class SkuTests
{
    [Fact]
    public void Create_WithValidValue_ReturnsSuccess()
    {
        var result = Sku.Create("ABC-123");

        result.IsSuccess.Should().BeTrue();
    }

    [Fact]
    public void Create_WithEmptyValue_ReturnsFailure()
    {
        var result = Sku.Create(string.Empty);

        result.IsFailure.Should().BeTrue();
        result.Error.Code.Should().Be("SKU_EMPTY");
    }

    [Fact]
    public void Create_WithWhitespaceOnly_ReturnsFailure()
    {
        var result = Sku.Create("   ");

        result.IsFailure.Should().BeTrue();
        result.Error.Code.Should().Be("SKU_EMPTY");
    }

    [Fact]
    public void Create_WithValueExceeding50Chars_ReturnsFailure()
    {
        var longValue = new string('A', 51);

        var result = Sku.Create(longValue);

        result.IsFailure.Should().BeTrue();
        result.Error.Code.Should().Be("SKU_TOO_LONG");
    }

    [Fact]
    public void Create_TrimsAndUppercasesValue()
    {
        var result = Sku.Create("  abc-123  ");

        result.IsSuccess.Should().BeTrue();
        result.Value.Value.Should().Be("ABC-123");
    }

    [Fact]
    public void Equality_TwoSkusWithSameValue_AreEqual()
    {
        var sku1 = Sku.Create("PROD-001").Value;
        var sku2 = Sku.Create("PROD-001").Value;

        sku1.Should().Be(sku2);
        (sku1 == sku2).Should().BeTrue();
    }

    [Fact]
    public void Equality_TwoSkusWithDifferentValue_AreNotEqual()
    {
        var sku1 = Sku.Create("PROD-001").Value;
        var sku2 = Sku.Create("PROD-002").Value;

        sku1.Should().NotBe(sku2);
        (sku1 != sku2).Should().BeTrue();
    }

    [Fact]
    public void Operators_EqualityAndInequality_WorkCorrectly()
    {
        var sku1 = Sku.Create("SKU-A").Value;
        var sku2 = Sku.Create("SKU-A").Value;
        var sku3 = Sku.Create("SKU-B").Value;

        (sku1 == sku2).Should().BeTrue();
        (sku1 != sku2).Should().BeFalse();
        (sku1 == sku3).Should().BeFalse();
        (sku1 != sku3).Should().BeTrue();
    }

    [Fact]
    public void ToString_ReturnsUppercasedValue()
    {
        var sku = Sku.Create("abc-001").Value;

        sku.ToString().Should().Be("ABC-001");
    }
}
