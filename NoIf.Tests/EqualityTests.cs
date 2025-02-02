namespace NoIf.Tests;

public class EqualityTests
{
	[Fact]
	public void NoneIsNone()
	{
		Result.None.Should().Be(Result.None);
	}

	[Fact]
	public void SimpleErrorWorks()
	{
		var error = DoStuff();
		var result = error.Then<bool>(x => x.StartsWith("a"));

		result.Should().Be(new Error("wrong!"));

		static Result<string> DoStuff() => new Error("wrong!");
	}

	[Fact]
	public void SameErrorsAreEqual()
	{
		new Error("Some error", new Error("Some inner error", new Error("Very inner error")), new Error("Another inner error"))
			.Should().Be(new Error("Some error", new Error("Some inner error", new Error("Very inner error")), new Error("Another inner error")));
	}

	[Fact]
	public void WhenErrorMessagesDiffers_ErrorsAreNotEqual()
	{
		new Error("Some error")
			.Should().NotBe(new Error("Some issue"));
	}

	[Fact]
	public void WhenOneErrorHasEmptyInnerErrors_ErrorsAreNotEqual()
	{
		new Error("Some error", new Error("Some inner error"), new Error("Another inner error"))
			.Should().NotBe(new Error("Some error"));
	}

	[Fact]
	public void WhenInnerErrorsHaveDifferentItemsCount_ErrorsAreNotEqual()
	{
		new Error("Some error", new Error("Some inner error"), new Error("Another inner error"), new Error("Additional error"))
			.Should().NotBe(new Error("Some error", new Error("Some inner error"), new Error("Another inner error")));
	}

	[Fact]
	public void WhenInnerErrorsHaveDifferentMessage_ErrorsAreNotEqual()
	{
		new Error("Some error", new Error("Some inner errr"), new Error("Another inner error"))
			.Should().NotBe(new Error("Some error", new Error("Some inner error"), new Error("Another inner error")));
	}
}