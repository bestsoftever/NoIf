/*
 * Requirements:
 * parameterless constructor MUST NOT be exposed - nobody can't create a result which have no initial value
 *      so we can't use struct
 * errors SHOULD be able to pass through the whole flow, without necessity to re-mapping - it means errors can't be generic?
 * implicit conversions
 * handle exceptions in Then
 */

/*
 * - sync flow for reference types
 * - sync flow for value types
 * - unit type
 * - async flow
 * - collection flow
 * - combining errors
 */

public abstract class Result<T> //: IWrong3
{
    //public abstract T Value { get; }

    //public static implicit operator T(Result3<T> result) => result.Value;

    //public static implicit operator Right3<T>(T data) => new Right3<T>(data);

    //protected Result3(Wrong3 error)
    //{
    //    Error = error;
    //}

    //protected Result3(T data)
    //{
    //    Value = data;
    //}

    //public static Right3<T> Right(T value)
    //{
    //    return new Right3<T>(value);
    //}

    //public static Wrong3 Wrong(string message)
    //{
    //    return new Wrong3(message);
    //}

    public static implicit operator Result<T>(Wrong wrong) => Wrong<T>.From(wrong);

    public static implicit operator Result<T>(T data) => new Right<T>(data);
}

public class Right<T> : Result<T>
{
    public Right(T value)
    {
        Value = value;
    }

    public T Value { get; }

    public static implicit operator Right<T>(T data) => new Right<T>(data);
}

public class Wrong<T> : Result<T>
{
    public string Message { get; init; }

    public Wrong(string message)
    {
        Message = message;
    }

    public static Wrong<T> From<TInner>(Wrong<TInner> innerWrong)
    {
        return new Wrong<T>(innerWrong.Message);
    }

    public static Wrong<T> From(Wrong innerWrong)
    {
        return new Wrong<T>(innerWrong.Message);
    }
}

public class Wrong
{
    public string Message { get; init; }

    public Wrong(string message)
    {
        Message = message;
    }
}

public static class ResultExtensions
{
    public static Result<TOutput> Then<TInput, TOutput>(this Result<TInput> result, Func<TInput, Result<TOutput>> func)
    {
        return result switch
        {
            Wrong wrong => wrong,
            Right<TInput> right => func(right.Value),
            _ => new Wrong<TOutput>("Never happened!"),
        };
    }
}
