namespace NetInventory.Domain.Common;

public class Result
{
    protected Result(bool isSuccess, Error error)
    {
        if (isSuccess && error != Error.None)
            throw new InvalidOperationException();
        if (!isSuccess && error == Error.None)
            throw new InvalidOperationException();

        IsSuccess = isSuccess;
        Error = error;
    }

    public bool IsSuccess { get; }
    public bool IsFailure => !IsSuccess;
    public Error Error { get; }

    public static Result Success() => new(true, Error.None);
    public static Result Failure(Error error) => new(false, error);

    public static Result<TValue> Success<TValue>(TValue value) => new(value, true, Error.None);
    public static Result<TValue> Failure<TValue>(Error error) => new(default!, false, error);
}

public sealed class Result<TValue>(TValue value, bool isSuccess, Error error)
    : Result(isSuccess, error)
{
    private readonly TValue _value = value;

    public TValue Value => IsSuccess
        ? _value
        : throw new InvalidOperationException(NetInventory.Resources.Messages.Error_FailedResultAccess);
}
