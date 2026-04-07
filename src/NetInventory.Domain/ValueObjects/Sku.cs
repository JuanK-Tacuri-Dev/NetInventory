using NetInventory.Domain.Common;

namespace NetInventory.Domain.ValueObjects;

public sealed class Sku : IEquatable<Sku>
{
    public string Value { get; }

    private Sku(string value) => Value = value;

    public static Result<Sku> Create(string value)
    {
        if (string.IsNullOrWhiteSpace(value))
            return Result.Failure<Sku>(Error.Sku.Empty);

        value = value.Trim().ToUpperInvariant();

        if (value.Length > 50)
            return Result.Failure<Sku>(Error.Sku.TooLong);

        return Result.Success(new Sku(value));
    }

    public bool Equals(Sku? other) => other is not null && Value == other.Value;
    public override bool Equals(object? obj) => obj is Sku sku && Equals(sku);
    public override int GetHashCode() => Value.GetHashCode();
    public override string ToString() => Value;

    public static bool operator ==(Sku? left, Sku? right) => Equals(left, right);
    public static bool operator !=(Sku? left, Sku? right) => !Equals(left, right);
}
