/*
 * Requirements:
 * parameterless constructor MUST NOT be exposed - nobody can't create a result which have no initial value
 *      so we can't use struct
 * errors SHOULD be able to pass through the whole flow, without necessity to re-mapping - it means errors can't be generic?
 * implicit conversions
 * handle exceptions in Then
 */

/*
 * - unit type
 * - composite errors
 * - context (nested Then)
 * - collection flow (ThenForeach) which will return composite error
 */

namespace IfLess;

public abstract class Result<TRight>
{
    public abstract Result<TOutput> Then<TOutput>(Func<TRight, Result<TOutput>> func);
    public abstract Task<Result<TOutput>> Then<TOutput>(Func<TRight, Task<Result<TOutput>>> func);

    //public static implicit operator Result<TRight>(Wrong<TIn> wrong) => new Result<TRight>(wrong.Message);

    public static implicit operator Result<TRight>(Error error) => new Wrong<TRight>(error);

    public static implicit operator Result<TRight>(TRight data) => new Right<TRight>(data);

    protected Result() { }
}


public sealed class Right<TRight> : Result<TRight>
{
    public TRight Value { get; }

    public Right(TRight value)
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



//internal interface INone
//{
//    None None { get; }
//}

//internal sealed class None<TRight> : Result<TRight>
//{
//    public override Result<TOutput> Then<TOutput>(Func<TRight, Result<TOutput>> func)
//    {
//        throw new NotImplementedException();
//    }

//    public override Task<Result<TOutput>> Then<TOutput>(Func<TRight, Task<Result<TOutput>>> func)
//    {
//        throw new NotImplementedException();
//    }
//}

public class None //: Result<None>
{
    //public override Result<TOutput> Then<TOutput>(Func<None, Result<TOutput>> func)
    //{
    //    throw new NotImplementedException();
    //}

    //public override Task<Result<TOutput>> Then<TOutput>(Func<None, Task<Result<TOutput>>> func)
    //{
    //    throw new NotImplementedException();
    //}

    //public static implicit operator Result<TRight>(Error error) => new Wrong<TRight>(error);
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

// TODO: add another errors
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




//// for "void" scenarios
//public class Result : Result<ValueTuple>
//{
//    public static readonly Result Empty = new();

//    public static implicit operator Result(Wrong wrong) => wrong;

//    private Result()
//    {
//    }
//}

// internal?
//public class Right<T> : Result<T>
//{
//    public Right(T value)
//    {
//        Value = value;
//    }

//    public T Value { get; }

//    public static implicit operator Right<T>(T data) => new Right<T>(data);
//}

//internal class Wrong<T> : Result<T>
//{
//    public string Message { get; init; }

//    public Wrong(string message)
//    {
//        Message = message;
//    }

//    public static Wrong<T> From(Wrong innerWrong)
//    {
//        return new Wrong<T>(innerWrong.Message);
//    }

//    public static Wrong<T> From<TInner>(Wrong<TInner> innerWrong)
//    {
//        return new Wrong<T>(innerWrong.Message);
//    }
//}

//public class Wrong
//{
//    public string Message { get; init; }

//    public Wrong(string message)
//    {
//        Message = message;
//    }
//}

public static class ResultExtensions
{
    //public static Result<TOutput> Then<TInput, TOutput>(this Result<TInput> result,
    //    Func<TInput, Result<TOutput>> func)
    //{
    //    var x = typeof(TInput).Name;

    //    return result switch
    //    {
    //        Result<TInput>.Right<TInput> right => func(right.Value),
    //        Result<TInput>.Wrong<TInput> wrong => wrong,


    //        //Wrong<TInput> wrong => Wrong<TOutput>.From(wrong),
    //        //Right<TInput> right => func(right.Value),
    //        _ => throw new InvalidOperationException("This should never happened."),
    //    };
    //}

    //public static async Task<Result<TOutput>> Then<TInput, TOutput>(this Result<TInput> result,
    //    Func<TInput, Task<Result<TOutput>>> func)
    //{
    //    return result switch
    //    {
    //        Wrong wrong => Wrong<TOutput>.From(wrong),
    //        Wrong<TInput> wrong => Wrong<TOutput>.From(wrong),
    //        Right<TInput> right => await func(right.Value),
    //        _ => throw new InvalidOperationException("This should never happened."),
    //    };
    //}

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