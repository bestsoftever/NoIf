namespace IfLess;

/*
 * - sync flow
 * - unit type
 * - async flow
 * - collection flow
 * - combining errors
 */

public static class ResultExtensions
{
    public static Result Then<TValue>(this Result result, Func<TValue, Result> func)
    {
        return result is Valid<TValue> valid ? func(valid.Value) : result;
    }
}

public abstract class Result
{
    public abstract bool IsError { get; }
}

public class Valid<TValue> : Result
{
    public override bool IsError => false;

    public TValue Value;

    public Valid(TValue data)
    {
        Value = data;
    }

    public static implicit operator Valid<TValue>(TValue data) => new Valid<TValue>(data);
}

public class Error : Result
{
    public override bool IsError => false;

    public string Message { get; init; }

    public Error(string message)
    {
        Message = message;
    }
}