using System;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using static System.Collections.Specialized.BitVector32;

namespace IfLess;

public abstract class Result<TRight>
{
    public abstract Result<TOutput> Then<TOutput>(Func<TRight, Result<TOutput>> func);

    public abstract Task<Result<TOutput>> Then<TOutput>(Func<TRight, Task<Result<TOutput>>> func);

    public abstract Result<TRight> Swap<TToSwap>(Func<TToSwap, Result<TRight>> func);

    public abstract Task<Result<TRight>> Swap<TToSwap>(Func<TToSwap, Task<Result<TRight>>> func);

    public abstract Result<TRight> Act<TToAct>(Action<TToAct> action);

    //public abstract Task<Result<TRight>> ActAsync<TToAct>(Action<TToAct> action) where TToAct : class;
    //{
    //    return (await task).Act(action);
    //}

    // public abstract Result<TRight> Handle<TInput>(Func<Result<TInput>, Result<TRight>> func);

    // public abstract Result<TRight> Handle2<TInput>(Func<Result<TInput>, Result<TRight>> func);

    //public abstract Result<TRight> ThenError<TError>(Func<Error, Result<TRight>> func) where TError : Error;

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

    public override Result<TRight> Swap<TToSwap>(Func<TToSwap, Result<TRight>> func)
    {
        return Value is TToSwap valueToSwap ? func(valueToSwap) : Value;
    }

    public override async Task<Result<TRight>> Swap<TToSwap>(Func<TToSwap, Task<Result<TRight>>> func)
    {
        return Value is TToSwap valueToSwap ? await func(valueToSwap) : Value;
    }

    public override Result<TRight> Act<TToAct>(Action<TToAct> action)
    {
        if (Value is TToAct valueToAct)
        {
            action(valueToAct);
        }

        return Value;
    }

    //public Result<TRight> Act<TToAct>(Action<Result<TRight>> func) where TToAct : class;
    //    {
    //        Error HandleError(Error error)
    //{
    //    errorHandler(error);
    //    return error;
    //}

    //        return result switch
    //        {
    //            Right<TRight> right => right,
    //            Wrong<TRight> wrong => HandleError(wrong.Error),
    //            _ => throw new InvalidOperationException("It cannot happen!"),
    //        };
    //    }

    // public override Result<TRight> Handle<TInput>(Func<Result<TInput>, Result<TRight>> func)
    // {
    //     return Value!.GetType() == typeof(TInput) ? func(Value) : Value;
    // }

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



    //public override Task<Result<TRight>> ActAsync<TToAct>(Action<TToAct> action)
    //{
    //    throw new NotImplementedException();
    //}

    //public override Result<TRight> ThenError<TError>(Func<Error, Result<TRight>> func)
    //{
    //    return this;
    //}
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

    public override Result<TRight> Swap<TToSwap>(Func<TToSwap, Result<TRight>> func)
    {
        return Error is TToSwap errorToSwap ? func(errorToSwap) : Error;
    }

    public override async Task<Result<TRight>> Swap<TToSwap>(Func<TToSwap, Task<Result<TRight>>> func)
    {
        return Error is TToSwap errorToSwap ? await func(errorToSwap) : Error;
    }

    public override Result<TRight> Act<TToAct>(Action<TToAct> action)
    {
        if (Error is TToAct valueToAct)
        {
            action(valueToAct);
        }

        return Error;
    }

    // public override Result<TRight> Handle<TInput>(Func<Result<TInput>, Result<TRight>> func)
    // {
    //     throw new NotImplementedException();
    // }

    // public override Result<TRight> Handle<TInput>(Func<Result<TInput>, Result<TRight>> func)
    // {
    //     throw new NotImplementedException();
    // }

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

    //public override async Task<Result<TRight>> ActAsync<TToAct>(Action<TToAct> action)
    //{
    //    return (await task).Act(func);
    //}

    //public override Result<TRight> ThenError<TError>(Func<Error, Result<TRight>> func)
    //{
    //    return typeof(TError) == this.Error.GetType() ? func(this.Error) : this;
    //}
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

    //public static Task<Result<TRight>> Swap<TToSwap>(Func<TToSwap, Result<TRight>> func) where TToSwap : class;


    public static async Task<Result<TRight>> Act<TRight, TToAct>(this Task<Result<TRight>> task, Action<TToAct> action)
        //where TToAct : class
    {
        return (await task).Act(action);
    }

    public static async Task<Result<TRight>> Act<TRight, TToAct>(this Task<Result<TRight>> task, Action<Task<TToAct>> action)
        //where TToAct : class
    {
        return (await task).Act(action);
    }

    ///// <summary>
    ///// Invokes an action when Result is an error and passes the Result.
    ///// </summary>
    ///// <typeparam name="TRight"></typeparam>
    ///// <param name="result"></param>
    ///// <param name="errorHandler"></param>
    ///// <returns></returns>
    ///// <exception cref="InvalidOperationException"></exception>
    //public static Result<TRight> ActOnError<TRight>(this Result<TRight> result, Action<Error> errorHandler)
    //{
    //    Error HandleError(Error error)
    //    {
    //        errorHandler(error);
    //        return error;
    //    }

    //    return result switch
    //    {
    //        Right<TRight> right => right,
    //        Wrong<TRight> wrong => HandleError(wrong.Error),
    //        _ => throw new InvalidOperationException("It cannot happen!"),
    //    };
    //}

    //public static Result<TRight> ActOnError<TRight>(this Result<TRight> result, Action<Error> errorHandler)
    //{
    //    return (await task).Act<Error>(action);
    //}

    public static Result<TRight> ActOnError<TRight>(this Result<TRight> result, Action<Error> errorHandler)
    {
        return result.Act(errorHandler);
    }

    public static Task<Result<TRight>> ActOnError<TRight>(this Task<Result<TRight>> task, Action<Error> errorHandler)
    {
        return task.Act(errorHandler);
    }


    //public override Result<TRight> Swap<TToSwap>(Func<TToSwap, Result<TRight>> func)

    public static async Task<Result<TRight>> Swap<TRight, TToSwap>(this Task<Result<TRight>> task, Func<TToSwap, Result<TRight>> func)
    {
        return (await task).Swap(func);
    }

    public static async Task<Result<TRight>> Swap<TRight, TToSwap>(this Task<Result<TRight>> task, Func<TToSwap, Task<Result<TRight>>> func)
    {
        return await (await task).Swap(func);
    }


    ///// <summary>
    ///// Invokes an action when Result is an error and passes the Result.
    ///// </summary>
    ///// <typeparam name="TRight"></typeparam>
    ///// <param name="result"></param>
    ///// <param name="errorHandler"></param>
    ///// <returns></returns>
    ///// <exception cref="InvalidOperationException"></exception>
    //public static Result<TRight> Act<TRight, TToAct>(this Result<TRight> result, Action<TToAct> action)
    //{
    //    Result<TRight> HandleValue(Result<TRight> right)
    //    {
    //        if (right is TToAct valueToAct)
    //        {
    //            action(valueToAct);
    //        }

    //        return right;
    //    }

    //    Error HandleError(Error error)
    //    {
    //        if (error is TToAct valueToAct)
    //        {
    //            action(valueToAct);
    //        }

    //        return error;
    //    }

    //    return result switch
    //    {
    //        Right<TRight> right => HandleValue(right),
    //        Wrong<TRight> wrong => HandleError(wrong.Error),
    //        _ => throw new InvalidOperationException("It cannot happen!"),
    //    };
    //}

    ///// <summary>
    ///// Invokes an action when Result is an error and passes the Result.
    ///// </summary>
    ///// <typeparam name="TRight"></typeparam>
    ///// <param name="task"></param>
    ///// <param name="errorHandler"></param>
    ///// <returns></returns>
    ///// <exception cref="InvalidOperationException"></exception>
    //public static async Task<Result<TRight>> Act<TRight, TToAct>(this Task<Result<TRight>> task, Action<TToAct> action)
    //{
    //    Result<TRight> HandleValue(Result<TRight> right)
    //    {
    //        if (right is TToAct valueToAct)
    //        {
    //            action(valueToAct);
    //        }

    //        return right;
    //    }

    //    Error HandleError(Error error)
    //    {
    //        if (error is TToAct valueToAct)
    //        {
    //            action(valueToAct);
    //        }

    //        return error;
    //    }

    //    var result = await task;
    //    return result switch
    //    {
    //        Right<TRight> right => HandleValue(right),
    //        Wrong<TRight> wrong => HandleError(wrong.Error),
    //        _ => throw new InvalidOperationException("It cannot happen!"),
    //    };
    //}


    //public static Result<TRight> Swap<TRight, TToSwap>(this Func<Result<TRight>, Result<TRight>> func)
    //{
    //}

    // public static Result<TRight> Handle<TInput>(this Result<TRight> result, Func<Result<TInput>, Result<TRight>> func)
    // {
    //     return Value!.GetType() == typeof(TInput) ? func(Value) : Value;
    // }
}