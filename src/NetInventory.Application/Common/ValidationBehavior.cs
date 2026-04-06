using FluentValidation;
using NetInventory.Domain.Common;

namespace NetInventory.Application.Common;

public sealed class ValidationBehavior<TRequest>(IEnumerable<IValidator<TRequest>> validators)
{
    public Result Validate(TRequest request)
    {
        if (!validators.Any())
            return Result.Success();

        var context = new ValidationContext<TRequest>(request);

        var failures = validators
            .Select(v => v.Validate(context))
            .SelectMany(r => r.Errors)
            .Where(f => f is not null)
            .ToList();

        if (failures.Count == 0)
            return Result.Success();

        var first = failures[0];
        return Result.Failure(new Error(first.ErrorCode, first.ErrorMessage));
    }

    public Result<T> Validate<T>(TRequest request)
    {
        var result = Validate(request);
        return result.IsFailure
            ? Result.Failure<T>(result.Error)
            : Result.Success(default(T)!);
    }
}
