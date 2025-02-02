namespace NoIf;

/// <summary>
/// Base class for errors
/// </summary>
public class Error(string message, params Error[] innerErrors)
{
	public string Message { get; init; } = message;
	public IEnumerable<Error> InnerErrors { get; init; } = innerErrors;

	public override bool Equals(object? obj) => obj switch
	{
		Error error => Equals(this, error),
		IWrong wrong => Equals(this, wrong.Error),
		_ => false,
	};

	private static bool Equals(Error first, Error second) => first.Message == second.Message && first.InnerErrors.SequenceEqual(second.InnerErrors);

	public override int GetHashCode() => System.Runtime.CompilerServices.RuntimeHelpers.GetHashCode(this);
}
