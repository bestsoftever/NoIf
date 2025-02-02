namespace NoIf;

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

internal sealed class Right<TRight> : Result<TRight>
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

	public override int GetHashCode() => System.Runtime.CompilerServices.RuntimeHelpers.GetHashCode(this);
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
