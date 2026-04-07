using System.Globalization;
using NetInventory.Domain.Common;

namespace NetInventory.Domain.ValueObjects;

public sealed class Money : IEquatable<Money>
{
    public decimal Amount { get; }

    private Money(decimal amount) => Amount = amount;

    public static Result<Money> Create(decimal amount)
    {
        if (amount < 0)
            return Result.Failure<Money>(Error.Money.Negative);

        return Result.Success(new Money(amount));
    }

    public bool Equals(Money? other) => other is not null && Amount == other.Amount;
    public override bool Equals(object? obj) => obj is Money m && Equals(m);
    public override int GetHashCode() => Amount.GetHashCode();
    public override string ToString() => Amount.ToString("F2", CultureInfo.InvariantCulture);

    public static bool operator ==(Money? left, Money? right) => Equals(left, right);
    public static bool operator !=(Money? left, Money? right) => !Equals(left, right);
}
