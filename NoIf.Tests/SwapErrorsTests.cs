namespace NoIf.Tests;

public class SwapErrorsTests
{
	[Fact]
	public void SyncToSync_Valid()
	{
		Result<string> result = TestService.ReverseString("abc")
			.Swap<Error>(e => e.Message)
			.Then(s => TestService.ToUpperCase(s));

		result.Should().Be("CBA");
	}

	[Fact]
	public void SyncToSync_Error()
	{
		Result<string> result = TestService.ReverseString("    ")
			.Swap<Error>(e => e.Message)
			.Then(s => TestService.ToUpperCase(s));

		result.Should().Be("INPUT VALUE CAN'T BE EMPTY");
	}

	[Fact]
	public async Task SyncToAsync_Valid()
	{
		Result<string> result = await TestService.ReverseString("abc")
			.Swap<Error>(e => e.Message)
			.Then(s => TestService.ToUpperCaseAsync(s));

		result.Should().Be("CBA");
	}

	[Fact]
	public async Task SyncToAsync_Error()
	{
		Result<string> result = await TestService.ReverseString("    ")
			.Swap<Error>(e => e.Message)
			.Then(s => TestService.ToUpperCaseAsync(s));

		result.Should().Be("INPUT VALUE CAN'T BE EMPTY");
	}

	[Fact]
	public void SyncToNone_Valid()
	{
		Result<None> result = TestService.ReverseString("abc")
			.Swap<Error>(e => e.Message)
			.Then(s => TestService.DoNothing(s));

		result.Should().Be(Result.None);
	}

	[Fact]
	public void SyncToNone_Error()
	{
		Result<None> result = TestService.ReverseString("    ")
			.Swap<Error>(e => e.Message)
			.Then(s => TestService.DoNothing(s));

		result.Should().Be(Result.None);
	}

	[Fact]
	public async Task SyncToNoneAsync_Valid()
	{
		Result<None> result = await TestService.ReverseString("abc")
			.Swap<Error>(e => e.Message)
			.Then(s => TestService.DoNothingAsync(s));

		result.Should().Be(Result.None);
	}

	[Fact]
	public async Task SyncToNoneAsync_Error()
	{
		Result<None> result = await TestService.ReverseString("    ")
			.Swap<Error>(e => e.Message)
			.Then(s => TestService.DoNothingAsync(s));

		result.Should().Be(Result.None);
	}


	[Fact]
	public async Task AsyncToSync_Valid()
	{
		Result<string> result = await TestService.ReverseStringAsync("abc")
			.Swap<string, Error>(e => e.Message)
			.Then(s => TestService.ToUpperCase(s));

		result.Should().Be("CBA");
	}

	[Fact]
	public async Task AsyncToSync_Error()
	{
		Result<string> result = await TestService.ReverseStringAsync("    ")
			.Swap<string, Error>(e => e.Message)
			.Then(s => TestService.ToUpperCase(s));

		result.Should().Be("INPUT VALUE CAN'T BE EMPTY");
	}

	[Fact]
	public async Task AsyncToAsync_Valid()
	{
		Result<string> result = await TestService.ReverseStringAsync("abc")
			.Swap<string, Error>(e => e.Message)
			.Then(s => TestService.ToUpperCaseAsync(s));

		result.Should().Be("CBA");
	}

	[Fact]
	public async Task AsyncToAsync_Error()
	{
		Result<string> result = await TestService.ReverseStringAsync("    ")
			.Swap<string, Error>(e => e.Message)
			.Then(s => TestService.ToUpperCaseAsync(s));

		result.Should().Be("INPUT VALUE CAN'T BE EMPTY");
	}

	[Fact]
	public async Task AsyncToNone_Valid()
	{
		Result<None> result = await TestService.ReverseStringAsync("abc")
			.Swap<string, Error>(e => e.Message)
			.Then(s => TestService.DoNothing(s));

		result.Should().Be(Result.None);
	}

	[Fact]
	public async Task AsyncToNone_Error()
	{
		Result<None> result = await TestService.ReverseStringAsync("    ")
			.Swap<string, Error>(e => e.Message)
			.Then(s => TestService.DoNothing(s));

		result.Should().Be(Result.None);
	}

	[Fact]
	public async Task AsyncToNoneAsync_Valid()
	{
		Result<None> result = await TestService.ReverseStringAsync("abc")
			.Swap<string, Error>(e => e.Message)
			.Then(s => TestService.DoNothingAsync(s));

		result.Should().Be(Result.None);
	}

	[Fact]
	public async Task AsyncToNoneAsync_Error()
	{
		Result<None> result = await TestService.ReverseStringAsync("    ")
			.Swap<string, Error>(e => e.Message)
			.Then(s => TestService.DoNothingAsync(s));

		result.Should().Be(Result.None);
	}


	[Fact]
	public void NoneToSync_Valid()
	{
		string s = "cba";
		Result<string> result = TestService.DoNothing("abc")
			.Swap<Error>(e => Result.None)
			.Then(_ => TestService.ToUpperCase(s));

		result.Should().Be("CBA");
	}

	[Fact]
	public void NoneToSync_Error()
	{
		string s = "cba";
		Result<string> result = TestService.DoNothing("    ")
			.Swap<Error>(e => Result.None)
			.Then(_ => TestService.ToUpperCase(s));

		result.Should().Be("CBA");
	}

	[Fact]
	public async Task NoneToAsync_Valid()
	{
		string s = "cba";
		Result<string> result = await TestService.DoNothing("abc")
			.Swap<Error>(e => Result.None)
			.Then(_ => TestService.ToUpperCaseAsync(s));

		result.Should().Be("CBA");
	}

	[Fact]
	public async Task NoneToAsync_Error()
	{
		string s = "cba";
		Result<string> result = await TestService.DoNothing("    ")
			.Swap<Error>(e => Result.None)
			.Then(_ => TestService.ToUpperCaseAsync(s));

		result.Should().Be("CBA");
	}

	[Fact]
	public void NoneToNone_Valid()
	{
		string s = "cba";
		Result<None> result = TestService.DoNothing("abc")
			.Swap<Error>(e => Result.None)
			.Then(_ => TestService.DoNothing(s));

		result.Should().Be(Result.None);
	}

	[Fact]
	public void NoneToNone_Error()
	{
		string s = "cba";
		Result<None> result = TestService.DoNothing("    ")
			.Swap<Error>(e => Result.None)
			.Then(_ => TestService.DoNothing(s));

		result.Should().Be(Result.None);
	}

	[Fact]
	public async Task NoneToNoneAsync_Valid()
	{
		string s = "cba";
		Result<None> result = await TestService.DoNothing("abc")
			.Swap<Error>(e => Result.None)
			.Then(_ => TestService.DoNothingAsync(s));

		result.Should().Be(Result.None);
	}

	[Fact]
	public async Task NoneToNoneAsync_Error()
	{
		Result<None> result = await TestService.DoNothing("    ")
			.Swap<Error>(e => Result.None)
			.Then(_ => TestService.DoNothingAsync("cba"));

		result.Should().Be(Result.None);
	}


	[Fact]
	public async Task NoneAsyncToSync_Valid()
	{
		string s = "cba";
		Result<string> result = await TestService.DoNothingAsync("abc")
			.Swap<None, Error>(e => Result.None)
			.Then(_ => TestService.ToUpperCase(s));

		result.Should().Be("CBA");
	}

	[Fact]
	public async Task NoneAsyncToSync_Error()
	{
		string s = "cba";
		Result<string> result = await TestService.DoNothingAsync("    ")
			.Swap<None, Error>(e => Result.None)
			.Then(_ => TestService.ToUpperCase(s));

		result.Should().Be("CBA");
	}

	[Fact]
	public async Task NoneAsyncToAsync_Valid()
	{
		string s = "cba";
		Result<string> result = await TestService.DoNothingAsync("abc")
			.Swap<None, Error>(e => Result.None)
			.Then(_ => TestService.ToUpperCaseAsync(s));

		result.Should().Be("CBA");
	}

	[Fact]
	public async Task NoneAsyncToAsync_Error()
	{
		string s = "cba";
		Result<string> result = await TestService.DoNothingAsync("    ")
			.Swap<None, Error>(e => Result.None)
			.Then(_ => TestService.ToUpperCaseAsync(s));

		result.Should().Be("CBA");
	}

	[Fact]
	public async Task NoneAsyncToNone_Valid()
	{
		string s = "cba";
		Result<None> result = await TestService.DoNothingAsync("abc")
			.Swap<None, Error>(e => Result.None)
			.Then(_ => TestService.DoNothing(s));

		result.Should().Be(Result.None);
	}

	[Fact]
	public async Task NoneAsyncToNone_Error()
	{
		string s = "cba";
		Result<None> result = await TestService.DoNothingAsync("    ")
			.Swap<None, Error>(e => Result.None)
			.Then(_ => TestService.DoNothing(s));

		result.Should().Be(Result.None);
	}

	[Fact]
	public async Task NoneAsyncToNoneAsync_Valid()
	{
		string s = "cba";
		Result<None> result = await TestService.DoNothingAsync("abc")
			.Swap<None, Error>(e => Result.None)
			.Then(_ => TestService.DoNothingAsync(s));


		result.Should().Be(Result.None);
	}

	[Fact]
	public async Task NoneAsyncToNoneAsync_Error()
	{
		string s = "cba";
		Result<None> result = await TestService.DoNothingAsync("    ")
			.Swap<None, Error>(e => Result.None)
			.Then(_ => TestService.DoNothingAsync(s));

		result.Should().Be(Result.None);
	}
}
