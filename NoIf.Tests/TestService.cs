namespace NoIf.Tests;

static class TestService
{
	public const string ErrorMessage = "Input value can't be empty";

	public static Result<string> ReverseString(string input)
	{
		if (string.IsNullOrWhiteSpace(input))
		{
			return new Error(ErrorMessage);
		}

		return new string(input.Reverse().ToArray());
	}

	public static Result<string> ToUpperCase(string input)
	{
		if (string.IsNullOrWhiteSpace(input))
		{
			return new Error(ErrorMessage);
		}

		return input.ToUpperInvariant();
	}

	public static async Task<Result<string>> ReverseStringAsync(string input)
	{
		await Task.Yield();

		if (string.IsNullOrWhiteSpace(input))
		{
			return new Error(ErrorMessage);
		}

		return new string(input.Reverse().ToArray());
	}

	public static async Task<Result<string>> ToUpperCaseAsync(string input)
	{
		await Task.Yield();

		if (string.IsNullOrWhiteSpace(input))
		{
			return new Error(ErrorMessage);
		}

		return input.ToUpperInvariant();
	}

	public static Result<Unit> DoNothing(string input)
	{
		if (string.IsNullOrWhiteSpace(input))
		{
			return new Error(ErrorMessage);
		}

		return Unit.Default;
	}

	public static async Task<Result<Unit>> DoNothingAsync(string input)
	{
		await Task.Yield();

		if (string.IsNullOrWhiteSpace(input))
		{
			return new Error(ErrorMessage);
		}

		return Unit.Default;
	}
}
