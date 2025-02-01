using System.Runtime.CompilerServices;

namespace IfLess;

public abstract class Result<TRight>
{
    public abstract Result<TOutput> Then<TOutput>(Func<TRight, Result<TOutput>> func);

    public abstract Task<Result<TOutput>> Then<TOutput>(Func<TRight, Task<Result<TOutput>>> func);

    public abstract Result<TRight> Swap<TToSwap>(Func<TToSwap, Result<TRight>> func);

    public abstract Task<Result<TRight>> Swap<TToSwap>(Func<TToSwap, Task<Result<TRight>>> func);

    public abstract Result<TRight> Act<TToAct>(Action<TToAct> action);

    public static implicit operator Result<TRight>(Error error) => new Wrong<TRight>(error);

    public static implicit operator Result<TRight>(TRight data) => new Right<TRight>(data);

    protected Result() { }
}

public sealed class Right<TRight> : Result<TRight>
{
    public TRight Value { get; }

    internal Right(TRight value)
    {
        if (value == null)
        {
            throw new Exception("A literal that represents a non-existing reference is not a proper value of anything. If you'd like to represent the lack of a value, please use Result.None.");
        }

        Value = value;
    }

    public override Result<TOutput> Then<TOutput>(Func<TRight, Result<TOutput>> func)
        => func(Value);

    public override async Task<Result<TOutput>> Then<TOutput>(Func<TRight, Task<Result<TOutput>>> func)
        => await func(Value);

    public override Result<TRight> Swap<TToSwap>(Func<TToSwap, Result<TRight>> func)
        => Value is TToSwap valueToSwap ? func(valueToSwap) : Value;

    public override async Task<Result<TRight>> Swap<TToSwap>(Func<TToSwap, Task<Result<TRight>>> func)
        => Value is TToSwap valueToSwap ? await func(valueToSwap) : Value;

    public override Result<TRight> Act<TToAct>(Action<TToAct> action)
    {
        if (Value is TToAct valueToAct)
        {
            action(valueToAct);
        }

        return Value;
    }

    public override bool Equals(object? obj) => obj switch
    {
        TRight value => value.Equals(Value),
        Right<TRight> right => right.Value!.Equals(Value),
        _ => false,
    };

    public override int GetHashCode() => RuntimeHelpers.GetHashCode(this);
}

public sealed class None
{
    internal None() { }
}

public static class Result
{
    public static None None { get; } = new();
}

/// <summary>
/// Non-generic interface for non-generic type in generic hierarchy
/// </summary>
internal interface IWrong
{
    Error Error { get; }
}

/// <summary>
/// Wrapper over Error, so you can have non-generic Error for generic Result<T>
/// </summary>
/// <typeparam name="TRight"></typeparam>
internal sealed class Wrong<TRight>(Error error) : Result<TRight>, IWrong
{
    public Error Error { get; init; } = error;

    public override Result<TOutput> Then<TOutput>(Func<TRight, Result<TOutput>> func)
        => Error;

    public override async Task<Result<TOutput>> Then<TOutput>(Func<TRight, Task<Result<TOutput>>> func)
        => await Task.FromResult(Error);

    public override Result<TRight> Swap<TToSwap>(Func<TToSwap, Result<TRight>> func)
        => Error is TToSwap errorToSwap ? func(errorToSwap) : Error;

    public override async Task<Result<TRight>> Swap<TToSwap>(Func<TToSwap, Task<Result<TRight>>> func)
        => Error is TToSwap errorToSwap ? await func(errorToSwap) : Error;

    public override Result<TRight> Act<TToAct>(Action<TToAct> action)
    {
        if (Error is TToAct valueToAct)
        {
            action(valueToAct);
        }

        return Error;
    }

    public override bool Equals(object? obj) => obj switch
    {
        Error error => Error.Equals(error),
        Wrong<TRight> wrong => Error.Equals(wrong),
        _ => false,
    };

    public override int GetHashCode() => Error.GetHashCode();
}

/// <summary>
/// Base class for errors
/// </summary>
public class Error
{
    public string Message { get; }

    public IEnumerable<Error> InnerErrors { get; }

    public Error(string message, params Error[] innerErrors)
    {
        Message = message;
        InnerErrors = innerErrors;
    }

    public override bool Equals(object? obj) => obj switch
    {
        Error error => Equals(this, error),
        IWrong wrong => Equals(this, wrong.Error),
        _ => false,
    };

    private bool Equals(Error first, Error second) => first.Message == second.Message && first.InnerErrors.SequenceEqual(second.InnerErrors);

    public override int GetHashCode() => RuntimeHelpers.GetHashCode(this);
}

public static class ResultExtensions
{
    public static async Task<Result<TOutput>> Then<TInput, TOutput>(
        this Task<Result<TInput>> task, Func<TInput, Result<TOutput>> func) => (await task).Then(func);

    public static async Task<Result<TOutput>> Then<TInput, TOutput>(
        this Task<Result<TInput>> task, Func<TInput, Task<Result<TOutput>>> func) => await (await task).Then(func);

    public static async Task<Result<TRight>> Act<TRight, TToAct>(this Task<Result<TRight>> task, Action<TToAct> action) => (await task).Act(action);

    public static async Task<Result<TRight>> Act<TRight, TToAct>(this Task<Result<TRight>> task, Action<Task<TToAct>> action) => (await task).Act(action);

    public static Result<TRight> ActOnError<TRight>(this Result<TRight> result, Action<Error> errorHandler) => result.Act(errorHandler);

    public static Task<Result<TRight>> ActOnError<TRight>(this Task<Result<TRight>> task, Action<Error> errorHandler) => task.Act(errorHandler);

    public static async Task<Result<TRight>> Swap<TRight, TToSwap>(this Task<Result<TRight>> task, Func<TToSwap, Result<TRight>> func) => (await task).Swap(func);

    public static async Task<Result<TRight>> Swap<TRight, TToSwap>(this Task<Result<TRight>> task, Func<TToSwap, Task<Result<TRight>>> func) => await (await task).Swap(func);
}