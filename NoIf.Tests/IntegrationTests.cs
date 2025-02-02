namespace NoIf.Tests;

public class IntegrationTests
{
	// The stupidest class hierarchy possible
	public abstract class Animal(string name) { public string Name { get; init; } = name; }
	public sealed class Dog(string name) : Animal(name) { }
	public sealed class Cat(string name) : Animal(name) { }
	public sealed class ThisParrotIsDeadError : Error
	{
		public ThisParrotIsDeadError() : base(":(")
		{
		}
	}

	public static TheoryData<Result<Animal>, string, string, bool> Data => new()
	{
		{ new Dog("piesek"), "CAT IS PIESEK", string.Empty, true },
		{ new ThisParrotIsDeadError(), "CAT IS KOTEK", ":(", false }
	};

	[Theory, MemberData(nameof(Data))]
	public void Full_Flow_Works(Result<Animal> oracleResult, string expectedResult, string expectedError, bool expectedPies)
	{
		Result<Animal> GetRandomPetFromOracle() => oracleResult;
		string errorMessage = string.Empty;
		bool isPies = false;

		var result = GetRandomPetFromOracle()
			.Act<Error>(e => errorMessage = e.Message)
			.Act<Dog>(d => isPies = true)
			.Swap<ThisParrotIsDeadError>(e => new Cat("kotek"))
			.Swap<Dog>(d => new Cat(d.Name))
			.Then(c => TestService.ToUpperCase($"{c.GetType().Name} is {c.Name}"));

		result.Should().Be(expectedResult);
		errorMessage.Should().Be(expectedError);
		isPies.Should().Be(expectedPies);
	}
}
