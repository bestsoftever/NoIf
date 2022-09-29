/*
 * Requirements:
 * parameterless constructor MUST NOT be exposed - nobody can't create a result which have no initial value
 *      so we can't use struct
 * errors SHOULD be able to pass through the whole flow, without necessity to re-mapping - it means errors can't be generic?
 * implicit conversions
 */


/*
 * - sync flow for reference types
 * - sync flow for value types
 * - unit type
 * - async flow
 * - collection flow
 * - combining errors
 */


public static class ResultExtensions
{
    public static Result<TOutput> Then<TInput, TOutput>(this Result<TInput> result, Func<TInput, Result<TOutput>> func)
    {
        return result.IsError ? result.Error : func(result.Value);
    }
}

public class Result<TValue>
{
    public Error? Error { get; }
    public TValue? Value { get; }
    public bool IsError => Error != null;

    public Result(Error error)
    {
        Error = error;
    }

    public Result(TValue data)
    {
        Value = data;
    }

    public static implicit operator Result<TValue>(Error error) => new(error);

    public static implicit operator Result<TValue>(TValue data) => new(data);
}

public class Error
{
    public string Message { get; init; }

    public Error(string message)
    {
        Message = message;
    }
}
