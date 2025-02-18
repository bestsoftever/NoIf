namespace NoIf.Tests;

public class SwapTests
{
	[Fact]
	public void SyncToSync_Valid()
	{
		Result<string> result = TestService.ReverseString("abc")
			.Swap<string>(s => new Error(s))
			.Then(s => TestService.ToUpperCase(string.Empty));

		result.Should().Be(new Error("cba"));
	}

	[Fact]
	public void SyncToSync_Error()
	{
		Result<string> result = TestService.ReverseString("  ")
			.Swap<string>(s => new Error(s))
			.Then(s => TestService.ToUpperCase(string.Empty));

		result.Should().Be(new Error(TestService.ErrorMessage));
	}

	[Fact]
	public async Task SyncToAsync_Valid()
	{
		Result<string> result = await TestService.ReverseString("abc")
			.Swap<string>(s => new Error(s))
			.Then(s => TestService.ToUpperCaseAsync(string.Empty));

		result.Should().Be(new Error("cba"));
	}

	[Fact]
	public async Task SyncToAsync_Error()
	{
		Result<string> result = await TestService.ReverseString("    ")
			.Swap<string>(s => new Error(s))
			.Then(s => TestService.ToUpperCaseAsync(string.Empty));

		result.Should().Be(new Error(TestService.ErrorMessage));
	}

	[Fact]
	public void SyncToNone_Valid()
	{
		Result<None> result = TestService.ReverseString("abc")
			.Swap<string>(s => new Error(s))
			.Then(s => TestService.DoNothing(string.Empty));

		result.Should().Be(new Error("cba"));
	}

	[Fact]
	public void SyncToNone_Error()
	{
		Result<None> result = TestService.ReverseString("    ")
			.Swap<string>(s => new Error(s))
			.Then(s => TestService.DoNothing(string.Empty));

		result.Should().Be(new Error(TestService.ErrorMessage));
	}

	[Fact]
	public async Task SyncToNoneAsync_Valid()
	{
		Result<None> result = await TestService.ReverseString("abc")
			.Swap<string>(s => new Error(s))
			.Then(s => TestService.DoNothingAsync(string.Empty));

		result.Should().Be(new Error("cba"));
	}

	[Fact]
	public async Task SyncToNoneAsync_Error()
	{
		Result<None> result = await TestService.ReverseString("    ")
			.Swap<string>(s => new Error(s))
			.Then(s => TestService.DoNothingAsync(string.Empty));

		result.Should().Be(new Error(TestService.ErrorMessage));
	}

	[Fact]
	public async Task AsyncToSync_Valid()
	{
		Result<string> result = await TestService.ReverseStringAsync("abc")
			.Swap<string, string>(s => new Error(s))
			.Then(s => TestService.ToUpperCase(string.Empty));

		result.Should().Be(new Error("cba"));
	}

	[Fact]
	public async Task AsyncToSync_Error()
	{
		Result<string> result = await TestService.ReverseStringAsync("    ")
			.Swap<string, string>(s => new Error(s))
			.Then(s => TestService.ToUpperCase(string.Empty));

		result.Should().Be(new Error(TestService.ErrorMessage));
	}

	[Fact]
	public async Task AsyncToAsync_Valid()
	{
		Result<string> result = await TestService.ReverseStringAsync("abc")
			.Swap<string, string>(s => new Error(s))
			.Then(s => TestService.ToUpperCaseAsync(string.Empty));

		result.Should().Be(new Error("cba"));
	}

	[Fact]
	public async Task AsyncToAsync_Error()
	{
		Result<string> result = await TestService.ReverseStringAsync("    ")
			.Swap<string, string>(s => new Error(s))
			.Then(s => TestService.ToUpperCaseAsync(string.Empty));

		result.Should().Be(new Error(TestService.ErrorMessage));
	}

	[Fact]
	public async Task AsyncToNone_Valid()
	{
		Result<None> result = await TestService.ReverseStringAsync("abc")
			.Swap<string, string>(s => new Error(s))
			.Then(s => TestService.DoNothing(string.Empty));

		result.Should().Be(new Error("cba"));
	}

	[Fact]
	public async Task AsyncToNone_Error()
	{
		Result<None> result = await TestService.ReverseStringAsync("    ")
			.Swap<string, string>(s => new Error(s))
			.Then(s => TestService.DoNothing(string.Empty));

		result.Should().Be(new Error(TestService.ErrorMessage));
	}

	[Fact]
	public async Task AsyncToNoneAsync_Valid()
	{
		Result<None> result = await TestService.ReverseStringAsync("abc")
			.Swap<string, string>(s => new Error(s))
			.Then(s => TestService.DoNothingAsync(string.Empty));

		result.Should().Be(new Error("cba"));
	}

	[Fact]
	public async Task AsyncToNoneAsync_Error()
	{
		Result<None> result = await TestService.ReverseStringAsync("    ")
			.Swap<string, string>(s => new Error(s))
			.Then(s => TestService.DoNothingAsync(string.Empty));

		result.Should().Be(new Error(TestService.ErrorMessage));
	}

	[Fact]
	public void NoneToSync_Valid()
	{
		Result<string> result = TestService.DoNothing("abc")
			.Swap<None>(s => new Error("none"))
			.Then(_ => TestService.ToUpperCase(string.Empty));

		result.Should().Be(new Error("none"));
	}

	[Fact]
	public void NoneToSync_Error()
	{
		Result<string> result = TestService.DoNothing("    ")
			.Swap<None>(s => new Error("none"))
			.Then(_ => TestService.ToUpperCase(string.Empty));

		result.Should().Be(new Error(TestService.ErrorMessage));
	}

	[Fact]
	public async Task NoneToAsync_Valid()
	{
		Result<string> result = await TestService.DoNothing("abc")
			.Swap<None>(s => new Error("none"))
			.Then(_ => TestService.ToUpperCaseAsync(string.Empty));

		result.Should().Be(new Error("none"));
	}

	[Fact]
	public async Task NoneToAsync_Error()
	{
		Result<string> result = await TestService.DoNothing("    ")
			.Swap<None>(s => new Error("none"))
			.Then(_ => TestService.ToUpperCaseAsync(string.Empty));

		result.Should().Be(new Error(TestService.ErrorMessage));
	}

	[Fact]
	public void NoneToNone_Valid()
	{
		Result<None> result = TestService.DoNothing("abc")
			.Swap<None>(s => new Error("none"))
			.Then(_ => TestService.DoNothing(string.Empty));

		result.Should().Be(new Error("none"));
	}

	[Fact]
	public void NoneToNone_Error()
	{
		Result<None> result = TestService.DoNothing("    ")
			.Swap<string>(s => new Error(s))
			.Then(_ => TestService.DoNothing(string.Empty));

		result.Should().Be(new Error(TestService.ErrorMessage));
	}

	[Fact]
	public async Task NoneToNoneAsync_Valid()
	{
		Result<None> result = await TestService.DoNothing("abc")
			.Swap<None>(s => new Error("none"))
			.Then(_ => TestService.DoNothingAsync(string.Empty));

		result.Should().Be(new Error("none"));
	}

	[Fact]
	public async Task NoneToNoneAsync_Error()
	{
		Result<None> result = await TestService.DoNothing("    ")
			.Swap<string>(s => new Error(s))
			.Then(_ => TestService.DoNothingAsync(string.Empty));

		result.Should().Be(new Error(TestService.ErrorMessage));
	}

	[Fact]
	public async Task NoneAsyncToSync_Valid()
	{
		Result<string> result = await TestService.DoNothingAsync("abc")
			.Swap<None, None>(s => new Error("none"))
			.Then(_ => TestService.ToUpperCase(string.Empty));

		result.Should().Be(new Error("none"));
	}

	[Fact]
	public async Task NoneAsyncToSync_Error()
	{
		Result<string> result = await TestService.DoNothingAsync("    ")
			.Swap<None, None>(s => new Error("none"))
			.Then(_ => TestService.ToUpperCase(string.Empty));

		result.Should().Be(new Error(TestService.ErrorMessage));
	}

	[Fact]
	public async Task NoneAsyncToAsync_Valid()
	{
		Result<string> result = await TestService.DoNothingAsync("abc")
			.Swap<None, None>(s => new Error("none"))
			.Then(_ => TestService.ToUpperCaseAsync(string.Empty));

		result.Should().Be(new Error("none"));
	}

	[Fact]
	public async Task NoneAsyncToAsync_Error()
	{
		Result<string> result = await TestService.DoNothingAsync("    ")
			.Swap<None, None>(s => new Error("none"))
			.Then(_ => TestService.ToUpperCaseAsync(string.Empty));

		result.Should().Be(new Error(TestService.ErrorMessage));
	}

	[Fact]
	public async Task NoneAsyncToNone_Valid()
	{
		Result<None> result = await TestService.DoNothingAsync("abc")
			.Swap<None, None>(s => new Error("none"))
			.Then(_ => TestService.DoNothing(string.Empty));

		result.Should().Be(new Error("none"));
	}

	[Fact]
	public async Task NoneAsyncToNone_Error()
	{
		Result<None> result = await TestService.DoNothingAsync("    ")
			.Swap<None, None>(s => new Error("none"))
			.Then(_ => TestService.DoNothing(string.Empty));

		result.Should().Be(new Error(TestService.ErrorMessage));
	}

	[Fact]
	public async Task NoneAsyncToNoneAsync_Valid()
	{
		Result<None> result = await TestService.DoNothingAsync("abc")
			.Swap<None, None>(s => new Error("none"))
			.Then(_ => TestService.DoNothingAsync(string.Empty));

		result.Should().Be(new Error("none"));
	}

	[Fact]
	public async Task NoneAsyncToNoneAsync_Error()
	{
		Result<None> result = await TestService.DoNothingAsync("    ")
			.Swap<None, None>(s => new Error("none"))
			.Then(_ => TestService.DoNothingAsync(string.Empty));

		result.Should().Be(new Error(TestService.ErrorMessage));
	}
}
