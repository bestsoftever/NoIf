namespace NoIf;

public static class AsyncResultExtensions
{
	public static async Task<Result<TOutput>> Then<TInput, TOutput>(this Task<Result<TInput>> task, Func<TInput, Result<TOutput>> func)
		=> (await task).Then(func);

	public static async Task<Result<TOutput>> Then<TInput, TOutput>(this Task<Result<TInput>> task, Func<TInput, Task<Result<TOutput>>> func)
		=> await (await task).Then(func);

	public static async Task<Result<TRight>> Act<TRight, TToAct>(this Task<Result<TRight>> task, Action<TToAct> action)
		=> (await task).Act(action);

	public static async Task<Result<TRight>> Act<TRight, TToAct>(this Task<Result<TRight>> task, Action<Task<TToAct>> action)
		=> (await task).Act(action);

	public static Result<TRight> ActOnError<TRight>(this Result<TRight> result, Action<Error> errorHandler)
		=> result.Act(errorHandler);

	public static Task<Result<TRight>> ActOnError<TRight>(this Task<Result<TRight>> task, Action<Error> errorHandler)
		=> task.Act(errorHandler);

	public static async Task<Result<TRight>> Swap<TRight, TToSwap>(this Task<Result<TRight>> task, Func<TToSwap, Result<TRight>> func)
		=> (await task).Swap(func);

	public static async Task<Result<TRight>> Swap<TRight, TToSwap>(this Task<Result<TRight>> task, Func<TToSwap, Task<Result<TRight>>> func)
		=> await (await task).Swap(func);
}