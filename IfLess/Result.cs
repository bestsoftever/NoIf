﻿namespace IfLess;

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
            throw new Exception("NO.");
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
            Right<TRight> right => right.Value.Equals(Value),
            _ => false,
        };
    }

    public override int GetHashCode()
    {
        return Value.GetHashCode();
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

internal interface IWrong
{
    Error Error { get; }
}

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
            Error error => Error.Message == error.Message,
            Wrong<TRight> wrong => Error.Message == wrong.Error.Message,
            _ => false,
        };
    }
}

public class Error
{
    public string Message { get; }

    public Error(string message)
    {
        Message = message;
    }

    public override bool Equals(object? obj)
    {
        return obj switch
        {
            Error error => Message == error.Message,
            IWrong wrong => Message == wrong.Error.Message,
            _ => false,
        };
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
}