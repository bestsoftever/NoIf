namespace IfLess.Tests;

/*
Valid and Invalid for:

sync - sync
sync - async
sync - none
sync - noneasync

async - sync
async - async
async - none
async - noneasync

none - sync
none - async
none - none
none - noneasync

noneasync - sync
noneasync - async
noneasync - none
noneasync - noneasync
*/

public class SyncTests
{
    [Fact]
    public void ValidInput_PassedFromSyncToSync_ReturnsValue()
    {
        Result<string> result = TestService.ReverseString("abc")
            .Then(s => TestService.ToUpperCase(s));

        result.Should().Be("CBA");
    }

    [Fact]
    public async Task ValidInput_PassedFromSyncToAsync_ReturnsValue()
    {
        Result<string> result = await TestService.ReverseString("abc")
            .Then(s => TestService.ToUpperCaseAsync(s));

        result.Should().Be("CBA");
    }

    [Fact]
    public void ValidInput_PassedFromSyncToNone_ReturnsNone()
    {
        Result<None> result = TestService.ReverseString("abc")
            .Then(s => TestService.DoNothing(s));

        result.Should().Be(Result.None);
    }

    [Fact]
    public async Task ValidInput_PassedFromSyncToNoneAsync_ReturnsNone()
    {
        Result<None> result = await TestService.ReverseString("abc")
            .Then(s => TestService.DoNothingAsync(s));

        result.Should().Be(Result.None);
    }

    [Fact]
    public void InvalidInput_PassedFromSyncToSync_ReturnsError()
    {
        Result<string> result = TestService.ReverseString("")
            .Then(s => TestService.ToUpperCase(s));

        result.Should().Be(new Error("Input value can't be empty"));
    }

    [Fact]
    public async Task InvalidInput_PassedFromSyncToAsync_ReturnsError()
    {
        Result<string> result = await TestService.ReverseString("")
            .Then(s => TestService.ToUpperCaseAsync(s));

        result.Should().Be(new Error("Input value can't be empty"));
    }

    [Fact]
    public void InvalidInput_PassedFromSyncToNone_ReturnsError()
    {
        Result<None> result = TestService.ReverseString("")
            .Then(s => TestService.DoNothing(s));

        result.Should().Be(new Error("Input value can't be empty"));
    }

    [Fact]
    public async Task InvalidInput_PassedFromSyncToNoneAsync_ReturnsNone()
    {
        Result<None> result = await TestService.ReverseString("")
            .Then(s => TestService.DoNothingAsync(s));

        result.Should().Be(new Error("Input value can't be empty"));
    }
}

public class AsyncTests
{
    [Fact]
    public async Task ValidInput_PassedFromAsyncToSync_ReturnsValue()
    {
        Result<string> result = await TestService.ReverseStringAsync("abc")
            .Then(s => TestService.ToUpperCase(s));

        result.Should().Be("CBA");
    }

    [Fact]
    public async Task ValidInput_PassedFromAsyncToAsync_ReturnsValue()
    {
        Result<string> result = await TestService.ReverseStringAsync("abc")
            .Then(s => TestService.ToUpperCaseAsync(s));

        result.Should().Be("CBA");
    }

    [Fact]
    public async Task ValidInput_PassedFromAsyncToNone_ReturnsValue()
    {
        Result<None> result = await TestService.ReverseStringAsync("abc")
            .Then(s => TestService.DoNothing(s));

        result.Should().Be(Result.None);
    }

    [Fact]
    public async Task ValidInput_PassedFromAsyncToNoneAsync_ReturnsValue()
    {
        Result<None> result = await TestService.ReverseStringAsync("abc")
            .Then(s => TestService.DoNothingAsync(s));

        result.Should().Be(Result.None);
    }

    [Fact]
    public async Task InvalidInput_PassedFromAsyncToSync_ReturnsError()
    {
        Result<string> result = await TestService.ReverseStringAsync("")
            .Then(s => TestService.ToUpperCase(s));

        result.Should().Be(new Error("Input value can't be empty"));
    }

    [Fact]
    public async Task InvalidInput_PassedFromAsyncToAsync_ReturnsError()
    {
        Result<string> result = await TestService.ReverseStringAsync("")
            .Then(s => TestService.ToUpperCaseAsync(s));

        result.Should().Be(new Error("Input value can't be empty"));
    }

    [Fact]
    public async Task InvalidInput_PassedFromAsyncToNone_ReturnsError()
    {
        Result<None> result = await TestService.ReverseStringAsync("")
            .Then(s => TestService.DoNothing(s));

        result.Should().Be(new Error("Input value can't be empty"));
    }

     [Fact]
    public async Task InvalidInput_PassedFromAsyncToNoneAsync_ReturnsError()
    {
        Result<None> result = await TestService.ReverseStringAsync("")
            .Then(s => TestService.DoNothingAsync(s));

        result.Should().Be(new Error("Input value can't be empty"));
    }
}

public class NoneTests
{
    [Fact]
    public void ValidInput_PassedFromNoneToSync_ReturnsValue()
    {
        Result<string> result = TestService.DoNothing("abc")
            .Then(s => TestService.ToUpperCase(s));

        result.Should().Be("CBA");
    }

    [Fact]
    public async Task ValidInput_PassedFromNoneToAsync_ReturnsValue()
    {
        Result<string> result = await TestService.ReverseStringAsync("abc")
            .Then(s => TestService.ToUpperCaseAsync(s));

        result.Should().Be("CBA");
    }

    [Fact]
    public async Task ValidInput_PassedFromNoneToNone_ReturnsValue()
    {
        Result<None> result = await TestService.ReverseStringAsync("abc")
            .Then(s => TestService.DoNothing(s));

        result.Should().Be(Result.None);
    }

    [Fact]
    public async Task InvalidInput_PassedFromNoneToSync_ReturnsError()
    {
        Result<string> result = await TestService.ReverseStringAsync("")
            .Then(s => TestService.ToUpperCase(s));

        result.Should().Be(new Error("Input value can't be empty"));
    }

    [Fact]
    public async Task InvalidInput_PassedFromNoneToAsync_ReturnsError()
    {
        Result<string> result = await TestService.ReverseStringAsync("")
            .Then(s => TestService.ToUpperCaseAsync(s));

        result.Should().Be(new Error("Input value can't be empty"));
    }

    [Fact]
    public async Task InvalidInput_PassedFromNoneToNone_ReturnsError()
    {
        Result<None> result = await TestService.ReverseStringAsync("")
            .Then(s => TestService.DoNothing(s));

        result.Should().Be(new Error("Input value can't be empty"));
    }
}

public class FlowTests
{
    [Fact]
    public void WhenValidInput_ReturnsValidResult()
    {
        Result<string> result = TestService.ReverseString("abc");

        result.Should().Be("cba");
    }

    [Fact]
    public void WhenFailedInput_ReturnsError()
    {
        Result<string> result = TestService.ReverseString("  ");

        result.Should().Be(new Error("Input value can't be empty"));
    }

    [Fact]
    public void Then_WhenValidInput_ProperlyPassedToNextMethod()
    {
        Result<string> result = TestService.ReverseString("abc")
            .Then(s => TestService.ToUpperCase(s));

        result.Should().Be("CBA");
    }

    [Fact]
    public void Then_WhenError_ProperlyPassesIt()
    {
        Result<string> result = TestService.ReverseString("    ")
            .Then(s => TestService.ToUpperCase(s));

        result.Should().Be(new Error("Input value can't be empty"));
    }


    [Fact]
    public void IfError_WhenValidInput_ProperlyPassedToNextMethod()
    {
        string errorMessage = string.Empty;
        Result<string> result = TestService.ReverseString("abc")
            .IfError(e => errorMessage = e.Message)
            .Then(s => TestService.ToUpperCase(s));

        result.Should().Be("CBA");
    }

    [Fact]
    public void IfError_WhenError_ThenItCanBeHandledAlso()
    {
        string errorMessage = string.Empty;
        Result<string> result = TestService.ReverseString("    ")
            .IfError(e => errorMessage = $"message logged: {e.Message}")
            .Then(s => TestService.ToUpperCase(s));

        errorMessage.Should().Be("message logged: Input value can't be empty");
        result.Should().Be(new Error("Input value can't be empty"));
    }
}

public class AsyncFlowTests
{
    [Fact]
    public async Task WhenValidInput_ReturnsValidResult()
    {
        Result<string> result = await TestService.ReverseStringAsync("abc");

        result.Should().Be("cba");
    }

    [Fact]
    public async Task WhenFailedInput_ReturnsError()
    {
        Result<string> result = await TestService.ReverseStringAsync("  ");

        result.Should().Be(new Error("Input value can't be empty"));
    }

    [Fact]
    public async Task WhenValidInput_ThenProperlyPassedToNextMethod()
    {
        Result<string> result = await TestService.ReverseStringAsync("abc")
            .Then(s => TestService.ToUpperCaseAsync(s));

        result.Should().Be("CBA");
    }

    [Fact]
    public async Task IfError_ThenProperlyPassesIt()
    {
        Result<string> result = await TestService.ReverseStringAsync("    ")
            .Then(s => TestService.ToUpperCaseAsync(s));

        result.Should().Be(new Error("Input value can't be empty"));
    }

    [Fact]
    public async Task IfError_WhenValidInput_ProperlyPassedToNextMethodAsync()
    {
        string errorMessage = string.Empty;
        Result<string> result = await TestService.ReverseStringAsync("abc")
            .IfError(e => errorMessage = e.Message)
            .Then(s => TestService.ToUpperCaseAsync(s));

        result.Should().Be("CBA");
    }

    [Fact]
    public async Task IfError_WhenError_ThenItCanBeHandledAlso()
    {
        string errorMessage = string.Empty;
        Result<string> result = await TestService.ReverseStringAsync("    ")
            .IfError(e => errorMessage = $"message logged: {e.Message}")
            .Then(s => TestService.ToUpperCaseAsync(s));

        errorMessage.Should().Be("message logged: Input value can't be empty");
        result.Should().Be(new Error("Input value can't be empty"));
    }
}

public class FirstSyncThenAsyncFlowTests
{
    [Fact]
    public async Task WhenValidInput_ThenProperlyPassedToNextMethodAsync()
    {
        Result<string> result = await TestService.ReverseString("abc")
            .Then(s => TestService.ToUpperCaseAsync(s));

        result.Should().Be("CBA");
    }

    [Fact]
    public async Task IfError_ThenProperlyPassesItAsync()
    {
        Result<string> result = await TestService.ReverseString("    ")
            .Then(s => TestService.ToUpperCaseAsync(s));

        result.Should().Be(new Error("Input value can't be empty"));
    }

    [Fact]
    public async Task IfError_WhenValidInput_ProperlyPassedToNextMethodAsync()
    {
        string errorMessage = string.Empty;
        Result<string> result = await TestService.ReverseString("abc")
            .IfError(e => errorMessage = e.Message)
            .Then(s => TestService.ToUpperCaseAsync(s));

        result.Should().Be("CBA");
    }

    [Fact]
    public async Task IfError_WhenError_ThenItCanBeHandledAlso()
    {
        string errorMessage = string.Empty;
        Result<string> result = await TestService.ReverseString("    ")
            .IfError(e => errorMessage = $"message logged: {e.Message}")
            .Then(s => TestService.ToUpperCaseAsync(s));

        errorMessage.Should().Be("message logged: Input value can't be empty");
        result.Should().Be(new Error("Input value can't be empty"));
    }
}

public class FirstAsyncThenSyncFlowTests
{
    [Fact]
    public async Task WhenValidInput_ThenProperlyPassedToNextMethodAsync()
    {
        Result<string> result = await TestService.ReverseStringAsync("abc")
            .Then(s => TestService.ToUpperCase(s));

        result.Should().Be("CBA");
    }

    [Fact]
    public async Task IfError_ThenProperlyPassesItAsync()
    {
        Result<string> result = await TestService.ReverseStringAsync("    ")
            .Then(s => TestService.ToUpperCase(s));

        result.Should().Be(new Error("Input value can't be empty"));
    }

    [Fact]
    public async Task IfError_WhenValidInput_ProperlyPassedToNextMethodAsync()
    {
        string errorMessage = string.Empty;
        Result<string> result = await TestService.ReverseStringAsync("abc")
            .IfError(e => errorMessage = e.Message)
            .Then(s => TestService.ToUpperCase(s));

        result.Should().Be("CBA");
    }

    [Fact]
    public async Task IfError_WhenError_ThenItCanBeHandledAlso()
    {
        string errorMessage = string.Empty;
        Result<string> result = await TestService.ReverseStringAsync("    ")
            .IfError(e => errorMessage = $"message logged: {e.Message}")
            .Then(s => TestService.ToUpperCase(s));

        errorMessage.Should().Be("message logged: Input value can't be empty");
        result.Should().Be(new Error("Input value can't be empty"));
    }
}

public class NoValueTests
{
    [Fact]
    public void WhenValidInput_ReturnsValidResult()
    {
        Result<None> result = TestService.DoNothing("abc");

        result.Should().Be(Result.None);
    }

    [Fact]
    public void WhenFailedInput_ReturnsError()
    {
        Result<None> result = TestService.DoNothing("  ");

        result.Should().Be(new Error("Input value can't be empty"));
    }

    [Fact]
    public void WhenValidInput_ThenNextMethodInvoked()
    {
        string s = "cba";
        Result<string> result = TestService.DoNothing("abc")
            .Then(_ => TestService.ToUpperCase(s));

        result.Should().Be("CBA");
    }

    [Fact]
    public void IfError_ThenProperlyPassesIt()
    {
        string s = "cba";
        Result<string> result = TestService.DoNothing("    ")
            .Then(_ => TestService.ToUpperCase(s));

        result.Should().Be(new Error("Input value can't be empty"));
    }

    [Fact]
    public async Task WhenValidInput_ThenAsync_AsyncMethodInvokedAsync()
    {
        string s = "cba";
        Result<string> result = await TestService.DoNothing("abc")
            .Then(_ => TestService.ToUpperCaseAsync(s));

        result.Should().Be("CBA");
    }

    [Fact]
    public async Task IfError_ThenAsync_ErrorProperlyPassedAsync()
    {
        string s = "cba";
        Result<string> result = await TestService.DoNothing("    ")
            .Then(_ => TestService.ToUpperCaseAsync(s));

        result.Should().Be(new Error("Input value can't be empty"));
    }

    [Fact]
    public async Task IfError_WhenValidInput_ProperlyPassedToNextAsyncMethodAsync()
    {
        string errorMessage = string.Empty;
        string s = "cba";
        Result<string> result = await TestService.DoNothing("abc")
            .IfError(e => errorMessage = e.Message)
            .Then(_ => TestService.ToUpperCaseAsync(s));

        result.Should().Be("CBA");
    }

    [Fact]
    public async Task IfError_WhenError_ThenItCanBeHandledAlsoAsync()
    {
        string errorMessage = string.Empty;
        string s = "cba";
        Result<string> result = await TestService.DoNothing("    ")
            .IfError(e => errorMessage = $"message logged: {e.Message}")
            .Then(_ => TestService.ToUpperCaseAsync(s));

        errorMessage.Should().Be("message logged: Input value can't be empty");
        result.Should().Be(new Error("Input value can't be empty"));
    }
}