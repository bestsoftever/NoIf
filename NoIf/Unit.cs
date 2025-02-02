namespace NoIf;

public sealed class Unit
{
	internal Unit() { }

	public static Unit Default { get; } = new();
}
