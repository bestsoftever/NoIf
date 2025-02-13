using Xunit.Sdk;

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

	public static TheoryData<Func<Result<Animal>>, string, string, bool> Data => new()
	{
		{ () => new Dog("piesek"), "CAT IS PIESEK", string.Empty, true },
		{ () => new ThisParrotIsDeadError(), "CAT IS KOTEK", ":(", false }
	};

	[Theory, MemberData(nameof(Data))]
	public void Full_Flow_Works(Func<Result<Animal>> GetRandomPetFromOracle, string expectedResult, string expectedError, bool expectedPies)
	{
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
	internal class ThisParrotIsDeadException : Exception
	{
		public ThisParrotIsDeadException() : base(":(") { }
	}

	public static TheoryData<Func<Animal>, string, string, bool> ClassicData => new()
	{
		{ () => new Dog("piesek"), "CAT IS PIESEK", string.Empty, true },
		{ () => throw new ThisParrotIsDeadException(), "CAT IS KOTEK", ":(", false }
	};

	[Theory, MemberData(nameof(ClassicData))]
	public void Classic_Flow_Works(Func<Animal> GetRandomPetFromOracle, string expectedResult, string expectedError, bool expectedPies)
	{
		string errorMessage = string.Empty;
		bool isPies = false;

		Animal animal = null!;
		try
		{
			animal = GetRandomPetFromOracle();
		}
		catch (ThisParrotIsDeadException pe)
		{
			errorMessage = pe.Message;
			animal = new Cat("kotek");
		}
		catch (Exception e)
		{
			errorMessage = e.Message;
		}

		if (animal is Dog d)
		{
			isPies = true;
			animal = new Cat(d.Name);
		}

		var result = TestService.ToUpperCase($"{animal.GetType().Name} is {animal.Name}");

		result.Should().Be(expectedResult);
		errorMessage.Should().Be(expectedError);
		isPies.Should().Be(expectedPies);
	}
}
