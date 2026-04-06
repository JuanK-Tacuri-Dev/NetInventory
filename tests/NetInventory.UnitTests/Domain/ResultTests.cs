using FluentAssertions;
using NetInventory.Domain.Common;

namespace NetInventory.UnitTests.Domain;

public class ResultTests
{
    [Fact]
    public void Success_IsSuccessTrue()
    {
        var result = Result.Success<string>("value");

        result.IsSuccess.Should().BeTrue();
        result.IsFailure.Should().BeFalse();
    }

    [Fact]
    public void Success_WithValue_ReturnsValue()
    {
        var result = Result.Success<int>(42);

        result.Value.Should().Be(42);
    }

    [Fact]
    public void Failure_IsSuccessFalse()
    {
        var result = Result.Failure<string>(Error.ProductNotFound);

        result.IsSuccess.Should().BeFalse();
        result.IsFailure.Should().BeTrue();
    }

    [Fact]
    public void Failure_AccessingValue_ThrowsInvalidOperationException()
    {
        var result = Result.Failure<string>(Error.ProductNotFound);

        var act = () => { var _ = result.Value; };

        act.Should().Throw<InvalidOperationException>();
    }

    [Fact]
    public void Failure_ContainsError()
    {
        var result = Result.Failure<string>(Error.SkuDuplicated);

        result.Error.Should().Be(Error.SkuDuplicated);
        result.Error.Code.Should().Be("SKU_DUPLICATED");
    }

    [Fact]
    public void ResultGeneric_Success_WithNullableValue_IsSuccessTrue()
    {
        string? nullableValue = null;
        var result = Result.Success<string?>(nullableValue);

        result.IsSuccess.Should().BeTrue();
        result.IsFailure.Should().BeFalse();
        result.Value.Should().BeNull();
    }

    [Fact]
    public void Result_Failure_WithCustomError_HasCorrectCodeAndMessage()
    {
        var customError = new Error("CUSTOM_CODE", "Custom error message");
        var result = Result.Failure(customError);

        result.IsFailure.Should().BeTrue();
        result.Error.Code.Should().Be("CUSTOM_CODE");
        result.Error.Message.Should().Be("Custom error message");
    }

    [Fact]
    public void ResultGeneric_Failure_IsFailureTrue()
    {
        var result = Result.Failure<int>(Error.InvalidQuantity);

        result.IsFailure.Should().BeTrue();
        result.IsSuccess.Should().BeFalse();
        result.Error.Should().Be(Error.InvalidQuantity);
    }
}
