namespace NoIf;

public sealed class None
{
	internal None() { }

	//public static None Result { get; } = new();
}

// TODO: maybe better to have None.Result?
public static class Result
{
	public static None None { get; } = new();
}