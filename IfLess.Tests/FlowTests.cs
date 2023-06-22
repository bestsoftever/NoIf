namespace IfLess.Tests;

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
    public void WhenError_WhenValidInput_ProperlyPassedToNextMethod()
    {
        string errorMessage = string.Empty;
        Result<string> result = TestService.ReverseString("abc")
            .WhenError(e => errorMessage = e.Message)
            .Then(s => TestService.ToUpperCase(s));

        result.Should().Be("CBA");
    }

    [Fact]
    public void WhenError_WhenError_ThenItCanBeHandledAlso()
    {
        string errorMessage = string.Empty;
        Result<string> result = TestService.ReverseString("    ")
            .WhenError(e => errorMessage = $"message logged: {e.Message}")
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
    public async Task WhenError_ThenProperlyPassesIt()
    {
        Result<string> result = await TestService.ReverseStringAsync("    ")
            .Then(s => TestService.ToUpperCaseAsync(s));

        result.Should().Be(new Error("Input value can't be empty"));
    }

    [Fact]
    public async Task WhenError_WhenValidInput_ProperlyPassedToNextMethodAsync()
    {
        string errorMessage = string.Empty;
        Result<string> result = await TestService.ReverseStringAsync("abc")
            .WhenError(e => errorMessage = e.Message)
            .Then(s => TestService.ToUpperCaseAsync(s));

        result.Should().Be("CBA");
    }

    [Fact]
    public async Task WhenError_WhenError_ThenItCanBeHandledAlso()
    {
        string errorMessage = string.Empty;
        Result<string> result = await TestService.ReverseStringAsync("    ")
            .WhenError(e => errorMessage = $"message logged: {e.Message}")
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
    public async Task WhenError_ThenProperlyPassesItAsync()
    {
        Result<string> result = await TestService.ReverseString("    ")
            .Then(s => TestService.ToUpperCaseAsync(s));

        result.Should().Be(new Error("Input value can't be empty"));
    }

    [Fact]
    public async Task WhenError_WhenValidInput_ProperlyPassedToNextMethodAsync()
    {
        string errorMessage = string.Empty;
        Result<string> result = await TestService.ReverseString("abc")
            .WhenError(e => errorMessage = e.Message)
            .Then(s => TestService.ToUpperCaseAsync(s));

        result.Should().Be("CBA");
    }

    [Fact]
    public async Task WhenError_WhenError_ThenItCanBeHandledAlso()
    {
        string errorMessage = string.Empty;
        Result<string> result = await TestService.ReverseString("    ")
            .WhenError(e => errorMessage = $"message logged: {e.Message}")
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
    public async Task WhenError_ThenProperlyPassesItAsync()
    {
        Result<string> result = await TestService.ReverseStringAsync("    ")
            .Then(s => TestService.ToUpperCase(s));

        result.Should().Be(new Error("Input value can't be empty"));
    }

    [Fact]
    public async Task WhenError_WhenValidInput_ProperlyPassedToNextMethodAsync()
    {
        string errorMessage = string.Empty;
        Result<string> result = await TestService.ReverseStringAsync("abc")
            .WhenError(e => errorMessage = e.Message)
            .Then(s => TestService.ToUpperCase(s));

        result.Should().Be("CBA");
    }

    [Fact]
    public async Task WhenError_WhenError_ThenItCanBeHandledAlso()
    {
        string errorMessage = string.Empty;
        Result<string> result = await TestService.ReverseStringAsync("    ")
            .WhenError(e => errorMessage = $"message logged: {e.Message}")
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
    public void WhenError_ThenProperlyPassesIt()
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
    public async Task WhenError_ThenAsync_ErrorProperlyPassedAsync()
    {
        string s = "cba";
        Result<string> result = await TestService.DoNothing("    ")
            .Then(_ => TestService.ToUpperCaseAsync(s));

        result.Should().Be(new Error("Input value can't be empty"));
    }

    [Fact]
    public async Task WhenError_WhenValidInput_ProperlyPassedToNextAsyncMethodAsync()
    {
        string errorMessage = string.Empty;
        string s = "cba";
        Result<string> result = await TestService.DoNothing("abc")
            .WhenError(e => errorMessage = e.Message)
            .Then(_ => TestService.ToUpperCaseAsync(s));

        result.Should().Be("CBA");
    }

    [Fact]
    public async Task WhenError_WhenError_ThenItCanBeHandledAlsoAsync()
    {
        string errorMessage = string.Empty;
        string s = "cba";
        Result<string> result = await TestService.DoNothing("    ")
            .WhenError(e => errorMessage = $"message logged: {e.Message}")
            .Then(_ => TestService.ToUpperCaseAsync(s));

        errorMessage.Should().Be("message logged: Input value can't be empty");
        result.Should().Be(new Error("Input value can't be empty"));
    }
}