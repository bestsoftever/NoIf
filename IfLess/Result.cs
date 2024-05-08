﻿using System.Linq;
using System.Runtime.CompilerServices;

namespace IfLess;

public abstract class Result<TRight>
{
    public abstract Result<TOutput> Then<TOutput>(Func<TRight, Result<TOutput>> func);

    public abstract Task<Result<TOutput>> Then<TOutput>(Func<TRight, Task<Result<TOutput>>> func);

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
    {
        return func(Value);
    }

    public override async Task<Result<TOutput>> Then<TOutput>(Func<TRight, Task<Result<TOutput>>> func)
    {
        return await func(Value);
    }

    public override bool Equals(object? obj)
    {
        return obj switch
        {
            TRight value => value.Equals(Value),
            Right<TRight> right => right.Value!.Equals(Value),
            _ => false,
        };
    }

    public override int GetHashCode()
    {
        return RuntimeHelpers.GetHashCode(this);
    }
}

public sealed class None
{
    internal None()
    {
    }
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
internal sealed class Wrong<TRight> : Result<TRight>, IWrong
{
    public Error Error { get; init; }

    public Wrong(Error error)
    {
        Error = error;
    }

    public override Result<TOutput> Then<TOutput>(Func<TRight, Result<TOutput>> func)
    {
        return Error;
    }

    public override async Task<Result<TOutput>> Then<TOutput>(Func<TRight, Task<Result<TOutput>>> func)
    {
        return await Task.FromResult(Error);
    }

    public override bool Equals(object? obj)
    {
        return obj switch
        {
            Error error => Error.Equals(error),
            Wrong<TRight> wrong => Error.Equals(wrong),
            _ => false,
        };
    }

    public override int GetHashCode()
    {
        return Error.GetHashCode();
    }
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

    public override bool Equals(object? obj)
    {
        return obj switch
        {
            Error error => Equals(this, error),
            IWrong wrong => Equals(this, wrong.Error),
            _ => false,
        };
    }

    private bool Equals(Error first, Error second)
    {
        return first.Message == second.Message && first.InnerErrors.SequenceEqual(second.InnerErrors);
    }

    public override int GetHashCode()
    {
        return RuntimeHelpers.GetHashCode(this);
    }
}

public static class ResultExtensions
{
    public static async Task<Result<TOutput>> Then<TInput, TOutput>(
        this Task<Result<TInput>> task, Func<TInput, Result<TOutput>> func)
    {
        return (await task).Then(func);
    }

    public static async Task<Result<TOutput>> Then<TInput, TOutput>(
        this Task<Result<TInput>> task, Func<TInput, Task<Result<TOutput>>> func)
    {
        return await (await task).Then(func);
    }

    public static Result<TRight> IfError<TRight>(
        this Result<TRight> result, Action<Error> errorHandler)
    {
        Error HandleError(Error error)
        {
            errorHandler(error);
            return error;
        }

        return result switch
        {
            Right<TRight> right => right,
            Wrong<TRight> wrong => HandleError(wrong.Error),
            _ => throw new InvalidOperationException("It cannot happen!"),
        };
    }

    public static async Task<Result<TRight>> IfError<TRight>(
        this Task<Result<TRight>> task, Action<Error> errorHandler)
    {
        Error HandleError(Error error)
        {
            errorHandler(error);
            return error;
        }

        var result = await task;
        return result switch
        {
            Right<TRight> right => right,
            Wrong<TRight> wrong => HandleError(wrong.Error),
            _ => throw new InvalidOperationException("It cannot happen!"),
        };
    }
}