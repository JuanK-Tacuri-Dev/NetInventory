using FluentAssertions;
using NetInventory.Domain.ValueObjects;

namespace NetInventory.UnitTests.Domain;

public class MoneyTests
{
    [Fact]
    public void Create_WithPositiveAmount_ReturnsSuccess()
    {
        var result = Money.Create(99.99m);

        result.IsSuccess.Should().BeTrue();
        result.Value.Amount.Should().Be(99.99m);
    }

    [Fact]
    public void Create_WithZeroAmount_ReturnsSuccess()
    {
        var result = Money.Create(0m);

        result.IsSuccess.Should().BeTrue();
        result.Value.Amount.Should().Be(0m);
    }

    [Fact]
    public void Create_WithNegativeAmount_ReturnsFailure()
    {
        var result = Money.Create(-1m);

        result.IsFailure.Should().BeTrue();
        result.Error.Code.Should().Be("MONEY_NEGATIVE");
    }

    [Fact]
    public void Equality_TwoMoneyWithSameAmount_AreEqual()
    {
        var money1 = Money.Create(50m).Value;
        var money2 = Money.Create(50m).Value;

        money1.Should().Be(money2);
        (money1 == money2).Should().BeTrue();
    }

    [Fact]
    public void Equality_TwoMoneyWithDifferentAmount_AreNotEqual()
    {
        var money1 = Money.Create(50m).Value;
        var money2 = Money.Create(75m).Value;

        money1.Should().NotBe(money2);
        (money1 != money2).Should().BeTrue();
    }

    [Fact]
    public void Operators_EqualityAndInequality_WorkCorrectly()
    {
        var money1 = Money.Create(25m).Value;
        var money2 = Money.Create(25m).Value;
        var money3 = Money.Create(99m).Value;

        (money1 == money2).Should().BeTrue();
        (money1 != money2).Should().BeFalse();
        (money1 == money3).Should().BeFalse();
        (money1 != money3).Should().BeTrue();
    }

    [Fact]
    public void Create_WithZeroAndPositive_BothSucceed()
    {
        var resultZero = Money.Create(0m);
        var resultSmall = Money.Create(0.01m);

        resultZero.IsSuccess.Should().BeTrue();
        resultZero.Value.Amount.Should().Be(0m);
        resultSmall.IsSuccess.Should().BeTrue();
        resultSmall.Value.Amount.Should().Be(0.01m);
    }

    [Fact]
    public void ToString_ReturnsFormattedAmount()
    {
        var money = Money.Create(10.5m).Value;

        money.ToString().Should().Be("10.50");
    }
}
