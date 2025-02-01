namespace NoIf.Tests;

public class ActOnErrorTests
{
    [Fact]
    public void SyncToSync_Valid()
    {
        string errorMessage = string.Empty;
        Result<string> result = TestService.ReverseString("abc")
            .ActOnError(e => errorMessage = $"message logged: {e.Message}")
            .Then(s => TestService.ToUpperCase(s));

        errorMessage.Should().BeEmpty();
        result.Should().Be("CBA");
    }

    [Fact]
    public void SyncToSync_Error()
    {
        string errorMessage = string.Empty;
        Result<string> result = TestService.ReverseString("    ")
            .ActOnError(e => errorMessage = $"message logged: {e.Message}")
            .Then(s => TestService.ToUpperCase(s));

        errorMessage.Should().Be("message logged: Input value can't be empty");
        result.Should().Be(new Error("Input value can't be empty"));
    }

    [Fact]
    public async Task SyncToAsync_Valid()
    {
        string errorMessage = string.Empty;
        Result<string> result = await TestService.ReverseString("abc")
            .ActOnError(e => errorMessage = $"message logged: {e.Message}")
            .Then(s => TestService.ToUpperCaseAsync(s));

        errorMessage.Should().BeEmpty();
        result.Should().Be("CBA");
    }

    [Fact]
    public async Task SyncToAsync_Error()
    {
        string errorMessage = string.Empty;
        Result<string> result = await TestService.ReverseString("    ")
            .ActOnError(e => errorMessage = $"message logged: {e.Message}")
            .Then(s => TestService.ToUpperCaseAsync(s));

        errorMessage.Should().Be("message logged: Input value can't be empty");
        result.Should().Be(new Error("Input value can't be empty"));
    }

    [Fact]
    public void SyncToNone_Valid()
    {
        string errorMessage = string.Empty;
        Result<None> result = TestService.ReverseString("abc")
            .ActOnError(e => errorMessage = $"message logged: {e.Message}")
            .Then(s => TestService.DoNothing(s));

        errorMessage.Should().BeEmpty();
        result.Should().Be(Result.None);
    }

    [Fact]
    public void SyncToNone_Error()
    {
        string errorMessage = string.Empty;
        Result<None> result = TestService.ReverseString("    ")
            .ActOnError(e => errorMessage = $"message logged: {e.Message}")
            .Then(s => TestService.DoNothing(s));

        errorMessage.Should().Be("message logged: Input value can't be empty");
        result.Should().Be(new Error("Input value can't be empty"));
    }

    [Fact]
    public async Task SyncToNoneAsync_Valid()
    {
        string errorMessage = string.Empty;
        Result<None> result = await TestService.ReverseString("abc")
            .ActOnError(e => errorMessage = $"message logged: {e.Message}")
            .Then(s => TestService.DoNothingAsync(s));

        errorMessage.Should().BeEmpty();
        result.Should().Be(Result.None);
    }

    [Fact]
    public async Task SyncToNoneAsync_Error()
    {
        string errorMessage = string.Empty;
        Result<None> result = await TestService.ReverseString("    ")
            .ActOnError(e => errorMessage = $"message logged: {e.Message}")
            .Then(s => TestService.DoNothingAsync(s));

        errorMessage.Should().Be("message logged: Input value can't be empty");
        result.Should().Be(new Error("Input value can't be empty"));
    }


    [Fact]
    public async Task AsyncToSync_Valid()
    {
        string errorMessage = string.Empty;
        Result<string> result = await TestService.ReverseStringAsync("abc")
            .ActOnError(e => errorMessage = $"message logged: {e.Message}")
            .Then(s => TestService.ToUpperCase(s));

        errorMessage.Should().BeEmpty();
        result.Should().Be("CBA");
    }

    [Fact]
    public async Task AsyncToSync_Error()
    {
        string errorMessage = string.Empty;
        Result<string> result = await TestService.ReverseStringAsync("    ")
            .ActOnError(e => errorMessage = $"message logged: {e.Message}")
            .Then(s => TestService.ToUpperCase(s));

        errorMessage.Should().Be("message logged: Input value can't be empty");
        result.Should().Be(new Error("Input value can't be empty"));
    }

    [Fact]
    public async Task AsyncToAsync_Valid()
    {
        string errorMessage = string.Empty;
        Result<string> result = await TestService.ReverseStringAsync("abc")
            .ActOnError(e => errorMessage = $"message logged: {e.Message}")
            .Then(s => TestService.ToUpperCaseAsync(s));

        errorMessage.Should().BeEmpty();
        result.Should().Be("CBA");
    }

    [Fact]
    public async Task AsyncToAsync_Error()
    {
        string errorMessage = string.Empty;
        Result<string> result = await TestService.ReverseStringAsync("    ")
            .ActOnError(e => errorMessage = $"message logged: {e.Message}")
            .Then(s => TestService.ToUpperCaseAsync(s));

        errorMessage.Should().Be("message logged: Input value can't be empty");
        result.Should().Be(new Error("Input value can't be empty"));
    }

    [Fact]
    public async Task AsyncToNone_Valid()
    {
        string errorMessage = string.Empty;
        Result<None> result = await TestService.ReverseStringAsync("abc")
            .ActOnError(e => errorMessage = $"message logged: {e.Message}")
            .Then(s => TestService.DoNothing(s));

        errorMessage.Should().BeEmpty();
        result.Should().Be(Result.None);
    }

    [Fact]
    public async Task AsyncToNone_Error()
    {
        string errorMessage = string.Empty;
        Result<None> result = await TestService.ReverseStringAsync("    ")
            .ActOnError(e => errorMessage = $"message logged: {e.Message}")
            .Then(s => TestService.DoNothing(s));

        errorMessage.Should().Be("message logged: Input value can't be empty");
        result.Should().Be(new Error("Input value can't be empty"));
    }

    [Fact]
    public async Task AsyncToNoneAsync_Valid()
    {
        string errorMessage = string.Empty;
        Result<None> result = await TestService.ReverseStringAsync("abc")
            .ActOnError(e => errorMessage = $"message logged: {e.Message}")
            .Then(s => TestService.DoNothingAsync(s));

        errorMessage.Should().BeEmpty();
        result.Should().Be(Result.None);
    }

    [Fact]
    public async Task AsyncToNoneAsync_Error()
    {
        string errorMessage = string.Empty;
        Result<None> result = await TestService.ReverseStringAsync("    ")
            .ActOnError(e => errorMessage = $"message logged: {e.Message}")
            .Then(s => TestService.DoNothingAsync(s));

        errorMessage.Should().Be("message logged: Input value can't be empty");
        result.Should().Be(new Error("Input value can't be empty"));
    }


    [Fact]
    public void NoneToSync_Valid()
    {
        string errorMessage = string.Empty;
        string s = "cba";
        Result<string> result = TestService.DoNothing("abc")
            .ActOnError(e => errorMessage = $"message logged: {e.Message}")
            .Then(_ => TestService.ToUpperCase(s));

        errorMessage.Should().BeEmpty();
        result.Should().Be("CBA");
    }

    [Fact]
    public void NoneToSync_Error()
    {
        string errorMessage = string.Empty;
        string s = "cba";
        Result<string> result = TestService.DoNothing("    ")
            .ActOnError(e => errorMessage = $"message logged: {e.Message}")
            .Then(_ => TestService.ToUpperCase(s));

        errorMessage.Should().Be("message logged: Input value can't be empty");
        result.Should().Be(new Error("Input value can't be empty"));
    }

    [Fact]
    public async Task NoneToAsync_Valid()
    {
        string errorMessage = string.Empty;
        string s = "cba";
        Result<string> result = await TestService.DoNothing("abc")
            .ActOnError(e => errorMessage = $"message logged: {e.Message}")
            .Then(_ => TestService.ToUpperCaseAsync(s));

        errorMessage.Should().BeEmpty();
        result.Should().Be("CBA");
    }

    [Fact]
    public async Task NoneToAsync_Error()
    {
        string errorMessage = string.Empty;
        string s = "cba";
        Result<string> result = await TestService.DoNothing("    ")
            .ActOnError(e => errorMessage = $"message logged: {e.Message}")
            .Then(_ => TestService.ToUpperCaseAsync(s));

        errorMessage.Should().Be("message logged: Input value can't be empty");
        result.Should().Be(new Error("Input value can't be empty"));
    }

    [Fact]
    public void NoneToNone_Valid()
    {
        string errorMessage = string.Empty;
        string s = "cba";
        Result<None> result = TestService.DoNothing("abc")
            .ActOnError(e => errorMessage = $"message logged: {e.Message}")
            .Then(_ => TestService.DoNothing(s));

        errorMessage.Should().BeEmpty();
        result.Should().Be(Result.None);
    }

    [Fact]
    public void NoneToNone_Error()
    {
        string errorMessage = string.Empty;
        string s = "cba";
        Result<None> result = TestService.DoNothing("    ")
            .ActOnError(e => errorMessage = $"message logged: {e.Message}")
            .Then(_ => TestService.DoNothing(s));

        errorMessage.Should().Be("message logged: Input value can't be empty");
        result.Should().Be(new Error("Input value can't be empty"));
    }

    [Fact]
    public async Task NoneToNoneAsync_Valid()
    {
        string errorMessage = string.Empty;
        string s = "cba";
        Result<None> result = await TestService.DoNothing("abc")
            .ActOnError(e => errorMessage = $"message logged: {e.Message}")
            .Then(_ => TestService.DoNothingAsync(s));

        errorMessage.Should().BeEmpty();
        result.Should().Be(Result.None);
    }

    [Fact]
    public async Task NoneToNoneAsync_Error()
    {
        string errorMessage = string.Empty;
        string s = "cba";
        Result<None> result = await TestService.DoNothing("    ")
            .ActOnError(e => errorMessage = $"message logged: {e.Message}")
            .Then(_ => TestService.DoNothingAsync(s));

        errorMessage.Should().Be("message logged: Input value can't be empty");
        result.Should().Be(new Error("Input value can't be empty"));
    }


    [Fact]
    public async Task NoneAsyncToSync_Valid()
    {
        string errorMessage = string.Empty;
        string s = "cba";
        Result<string> result = await TestService.DoNothingAsync("abc")
            .ActOnError(e => errorMessage = $"message logged: {e.Message}")
            .Then(_ => TestService.ToUpperCase(s));

        errorMessage.Should().BeEmpty();
        result.Should().Be("CBA");
    }

    [Fact]
    public async Task NoneAsyncToSync_Error()
    {
        string errorMessage = string.Empty;
        string s = "cba";
        Result<string> result = await TestService.DoNothingAsync("    ")
            .ActOnError(e => errorMessage = $"message logged: {e.Message}")
            .Then(_ => TestService.ToUpperCase(s));

        errorMessage.Should().Be("message logged: Input value can't be empty");
        result.Should().Be(new Error("Input value can't be empty"));
    }

    [Fact]
    public async Task NoneAsyncToAsync_Valid()
    {
        string errorMessage = string.Empty;
        string s = "cba";
        Result<string> result = await TestService.DoNothingAsync("abc")
            .ActOnError(e => errorMessage = $"message logged: {e.Message}")
            .Then(_ => TestService.ToUpperCaseAsync(s));

        errorMessage.Should().BeEmpty();
        result.Should().Be("CBA");
    }

    [Fact]
    public async Task NoneAsyncToAsync_Error()
    {
        string errorMessage = string.Empty;
        string s = "cba";
        Result<string> result = await TestService.DoNothingAsync("    ")
            .ActOnError(e => errorMessage = $"message logged: {e.Message}")
            .Then(_ => TestService.ToUpperCaseAsync(s));

        errorMessage.Should().Be("message logged: Input value can't be empty");
        result.Should().Be(new Error("Input value can't be empty"));
    }

    [Fact]
    public async Task NoneAsyncToNone_Valid()
    {
        string errorMessage = string.Empty;
        string s = "cba";
        Result<None> result = await TestService.DoNothingAsync("abc")
            .ActOnError(e => errorMessage = $"message logged: {e.Message}")
            .Then(_ => TestService.DoNothing(s));

        errorMessage.Should().BeEmpty();
        result.Should().Be(Result.None);
    }

    [Fact]
    public async Task NoneAsyncToNone_Error()
    {
        string errorMessage = string.Empty;
        string s = "cba";
        Result<None> result = await TestService.DoNothingAsync("    ")
            .ActOnError(e => errorMessage = $"message logged: {e.Message}")
            .Then(_ => TestService.DoNothing(s));

        errorMessage.Should().Be("message logged: Input value can't be empty");
        result.Should().Be(new Error("Input value can't be empty"));
    }

    [Fact]
    public async Task NoneAsyncToNoneAsync_Valid()
    {
        string errorMessage = string.Empty;
        string s = "cba";
        Result<None> result = await TestService.DoNothingAsync("abc")
            .ActOnError(e => errorMessage = $"message logged: {e.Message}")
            .Then(_ => TestService.DoNothingAsync(s));

        errorMessage.Should().BeEmpty();
        result.Should().Be(Result.None);
    }

    [Fact]
    public async Task NoneAsyncToNoneAsync_Error()
    {
        string errorMessage = string.Empty;
        string s = "cba";
        Result<None> result = await TestService.DoNothingAsync("    ")
            .ActOnError(e => errorMessage = $"message logged: {e.Message}")
            .Then(_ => TestService.DoNothingAsync(s));

        errorMessage.Should().Be("message logged: Input value can't be empty");
        result.Should().Be(new Error("Input value can't be empty"));
    }
}
