namespace IfLess.Tests;

/*
Valid:

sync - sync
sync - async
sync - none

async - sync
async - async
async - none

none - sync
none - async
none - none

Invalid:

sync - sync
sync - async
sync - none

async - sync
async - async
async - none

none - sync
none - async
none - none
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
    public async void ValidInput_PassedFromSyncToAsync_ReturnsValue()
    {
        Result<string> result = await TestService.ReverseString("abc")
            .Then(s => TestService.ToUpperCaseAsync(s));

        result.Should().Be("CBA");
    }

     [Fact]
    public void ValidInput_PassedFromSyncToNone_ReturnsValue()
    {
        Result<None> result = TestService.ReverseString("abc")
            .Then(s => TestService.DoNothing(s));

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
    public async void InvalidInput_PassedFromSyncToAsync_ReturnsError()
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
}


public class AsyncTests
{
    [Fact]
    public async void ValidInput_PassedFromAsyncToSync_ReturnsValue()
    {
        Result<string> result = await TestService.ReverseStringAsync("abc")
            .Then(s => TestService.ToUpperCase(s));

        result.Should().Be("CBA");
    }

    [Fact]
    public async void ValidInput_PassedFromAsyncToAsync_ReturnsValue()
    {
        Result<string> result = await TestService.ReverseStringAsync("abc")
            .Then(s => TestService.ToUpperCaseAsync(s));

        result.Should().Be("CBA");
    }

     [Fact]
    public async void ValidInput_PassedFromAsyncToNone_ReturnsValue()
    {
        Result<None> result = await TestService.ReverseStringAsync("abc")
            .Then(s => TestService.DoNothing(s));

        result.Should().Be(Result.None);
    }

     [Fact]
    public async void InvalidInput_PassedFromAsyncToSync_ReturnsError()
    {
        Result<string> result = await TestService.ReverseStringAsync("")
            .Then(s => TestService.ToUpperCase(s));

        result.Should().Be(new Error("Input value can't be empty"));
    }

    [Fact]
    public async void InvalidInput_PassedFromAsyncToAsync_ReturnsError()
    {
        Result<string> result = await TestService.ReverseStringAsync("")
            .Then(s => TestService.ToUpperCaseAsync(s));

        result.Should().Be(new Error("Input value can't be empty"));
    }

     [Fact]
    public async void InvalidInput_PassedFromAsyncToNone_ReturnsError()
    {
        Result<None> result = await TestService.ReverseStringAsync("")
            .Then(s => TestService.DoNothing(s));

        result.Should().Be(new Error("Input value can't be empty"));
    }
}

public class NoneTests
{
    [Fact]
    public void ValidInput_FromSyncToSync_Returns()
    {

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