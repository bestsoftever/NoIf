namespace IfLess.Tests;

public class FlowTests
{
    [Fact]
    public void WhenValidInput_ReturnsValidResult()
    {
        var result = TestService.ReverseString("abc");

        result.Should().Be("cba");
    }

    [Fact]
    public void WhenFailedInput_ReturnsError()
    {
        var result = TestService.ReverseString("  ");

        result.Should().Be(new Error("Input value can't be empty"));
    }

    [Fact]
    public void Then_WhenValidInput_ProperlyPassedToNextMethod()
    {
        var result = TestService.ReverseString("abc")
            .Then(s => TestService.ToUpperCase(s));

        result.Should().Be("CBA");
    }

    [Fact]
    public void Then_WhenError_ProperlyPassesIt()
    {
        var result = TestService.ReverseString("    ")
            .Then(s => TestService.ToUpperCase(s));

        result.Should().Be(new Error("Input value can't be empty"));
    }


    [Fact]
    public void ThenWithErrorSupport_WhenValidInput_ProperlyPassedToNextMethod()
    {
        string errorMessage = string.Empty;
        var result = TestService.ReverseString("abc")
            .Then(e => errorMessage = e.Message,
                  s => TestService.ToUpperCase(s));

        result.Should().Be("CBA");
    }

    [Fact]
    public void ThenWithErrorSupport_WhenError_ThenItCanBeHandledAlso()
    {
        string errorMessage = string.Empty;
        var result = TestService.ReverseString("    ")
            .Then(e => errorMessage = $"message logged: {e.Message}",
                  s => TestService.ToUpperCase(s));

        errorMessage.Should().Be("message logged: Input value can't be empty");
        result.Should().Be(new Error("Input value can't be empty"));
    }
}

public class AsyncFlowTests
{
    [Fact]
    public async Task WhenValidInput_ReturnsValidResult()
    {
        var result = await TestService.ReverseStringAsync("abc");

        result.Should().Be("cba");
    }

    [Fact]
    public async Task WhenFailedInput_ReturnsError()
    {
        var result = await TestService.ReverseStringAsync("  ");

        result.Should().Be(new Error("Input value can't be empty"));
    }

    [Fact]
    public async Task WhenValidInput_ThenProperlyPassedToNextMethod()
    {
        var result = await TestService.ReverseStringAsync("abc")
            .Then(s => TestService.ToUpperCaseAsync(s));

        result.Should().Be("CBA");
    }

    [Fact]
    public async Task WhenError_ThenProperlyPassesIt()
    {
        var result = await TestService.ReverseStringAsync("    ")
            .Then(s => TestService.ToUpperCaseAsync(s));

        result.Should().Be(new Error("Input value can't be empty"));
    }
}

public class FirstSyncThenAsyncFlowTests
{
    [Fact]
    public async Task WhenValidInput_ThenProperlyPassedToNextMethodAsync()
    {
        var result = await TestService.ReverseString("abc")
            .Then(s => TestService.ToUpperCaseAsync(s));

        result.Should().Be("CBA");
    }

    [Fact]
    public async Task WhenError_ThenProperlyPassesItAsync()
    {
        var result = await TestService.ReverseString("    ")
            .Then(s => TestService.ToUpperCaseAsync(s));

        result.Should().Be(new Error("Input value can't be empty"));
    }
}

public class FirstAsyncThenSyncFlowTests
{
    [Fact]
    public async Task WhenValidInput_ThenProperlyPassedToNextMethodAsync()
    {
        var result = await TestService.ReverseStringAsync("abc")
            .Then(s => TestService.ToUpperCase(s));

        result.Should().Be("CBA");
    }

    [Fact]
    public async Task WhenError_ThenProperlyPassesItAsync()
    {
        var result = await TestService.ReverseStringAsync("    ")
            .Then(s => TestService.ToUpperCase(s));

        result.Should().Be(new Error("Input value can't be empty"));
    }
}

public class NoValueTests
{
    [Fact]
    public void WhenValidInput_ReturnsValidResult()
    {
        var result = TestService.DoNothing("abc");

        result.Should().Be(Result.None);
    }

    [Fact]
    public void WhenFailedInput_ReturnsError()
    {
        var result = TestService.DoNothing("  ");

        result.Should().Be(new Error("Input value can't be empty"));
    }

    [Fact]
    public void WhenValidInput_ThenNextMethodInvoked()
    {
        string s = "cba";
        var result = TestService.DoNothing("abc")
            .Then(_ => TestService.ToUpperCase(s));

        result.Should().Be("CBA");
    }

    [Fact]
    public void WhenError_ThenProperlyPassesIt()
    {
        string s = "cba";
        var result = TestService.DoNothing("    ")
            .Then(_ => TestService.ToUpperCase(s));

        result.Should().Be(new Error("Input value can't be empty"));
    }
}