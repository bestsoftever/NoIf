﻿/*
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

public abstract class Result<T>
{
    public abstract Result<TOutput> Then<TInput, TOutput>(Func<TInput, Result<TOutput>> func);
    public abstract Task<Result<TOutput>> Then<TInput, TOutput>(Func<TInput, Task<Result<TOutput>>> func);

    public class Right<TRight> : Result<TRight>
    {
        public Right(T value)
        {
            Value = value;
        }

        public T Value { get; }

        //public static implicit operator Right<TRight>(TRight data) => new Right<TRight>(data);
    }

    public class Wrong<TWrong> : Result<TWrong>
    {
        public string Message { get; init; }

        public Wrong(string message)
        {
            Message = message;
        }

        //public static Wrong<T> From(Wrong innerWrong)
        //{
        //    return new Wrong<T>(innerWrong.Message);
        //}

        //public static Wrong<T> From<TInner>(Wrong<TInner> innerWrong)
        //{
        //    return new Wrong<T>(innerWrong.Message);
        //}
    }

    //public static implicit operator Result<T>(Wrong wrong) => Wrong<T>(wrong);

    public static implicit operator Result<T>(T data) => new Right<T>(data);

    private Result() { }
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