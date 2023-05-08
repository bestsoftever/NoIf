namespace IfLess.Tests;

public class FlowTests
{
    [Fact]
    public void WhenValidInput_ReturnsValidResult()
    {
        var result = TestService.ReverseString("abc");

        result.Should().BeEquivalentTo(new Right<string>("cba"));
    }

    [Fact]
    public void WhenFailedInput_ReturnsError()
    {
        var result = TestService.ReverseString("  ");

        result.Should().BeEquivalentTo(new Wrong("Input value can't be empty"));
    }

    [Fact]
    public void WhenValidInput_ThenProperlyPassedToNextMethod()
    {
        var result = TestService.ReverseString("abc")
            .Then(s => TestService.ToUpperCase(s));

        result.Should().BeEquivalentTo(new Right<string>("CBA"));
    }

    [Fact]
    public void WhenError_ThenProperlyPassesIt()
    {
        var result = TestService.ReverseString("    ")
            .Then(s => TestService.ToUpperCase(s));

        result.Should().BeEquivalentTo(new Wrong("Input value can't be empty"));
    }
}

public class AsyncFlowTests
{
    [Fact]
    public async Task WhenValidInput_ReturnsValidResult()
    {
        var result = await TestService.ReverseStringAsync("abc");

        result.Should().BeEquivalentTo(new Right<string>("cba"));
    }

    [Fact]
    public async Task WhenFailedInput_ReturnsError()
    {
        var result = await TestService.ReverseStringAsync("  ");

        result.Should().BeEquivalentTo(new Wrong("Input value can't be empty"));
    }

    [Fact]
    public async Task WhenValidInput_ThenProperlyPassedToNextMethod()
    {
        var result = await TestService.ReverseStringAsync("abc")
            .Then(s => TestService.ToUpperCaseAsync(s));

        result.Should().BeEquivalentTo(new Right<string>("CBA"));
    }

    [Fact]
    public async Task WhenError_ThenProperlyPassesIt()
    {
        var result = await TestService.ReverseStringAsync("    ")
            .Then(s => TestService.ToUpperCaseAsync(s));

        result.Should().BeEquivalentTo(new Wrong("Input value can't be empty"));
    }
}

public class FirstSyncThenAsyncFlowTests
{
    [Fact]
    public async Task WhenValidInput_ThenProperlyPassedToNextMethodAsync()
    {
        var result = await TestService.ReverseString("abc")
            .Then(s => TestService.ToUpperCaseAsync(s));

        result.Should().BeEquivalentTo(new Right<string>("CBA"));
    }

    [Fact]
    public async Task WhenError_ThenProperlyPassesItAsync()
    {
        var result = await TestService.ReverseString("    ")
            .Then(s => TestService.ToUpperCaseAsync(s));

        result.Should().BeEquivalentTo(new Wrong("Input value can't be empty"));
    }
}

public class FirstAsyncThenSyncFlowTests
{
    [Fact]
    public async Task WhenValidInput_ThenProperlyPassedToNextMethodAsync()
    {
        var result = await TestService.ReverseStringAsync("abc")
            .Then(s => TestService.ToUpperCase(s));

        result.Should().BeEquivalentTo(new Right<string>("CBA"));
    }

    [Fact]
    public async Task WhenError_ThenProperlyPassesItAsync()
    {
        var result = await TestService.ReverseStringAsync("    ")
            .Then(s => TestService.ToUpperCase(s));

        result.Should().BeEquivalentTo(new Wrong("Input value can't be empty"));
    }
}

//public class NoValueTests
//{
//    [Fact]
//    public void WhenValidInput_ReturnsValidResult()
//    {
//        var result = TestService.DoNothing("abc");

//        result.Should().Be(Result.Empty);
//    }

//    [Fact]
//    public void WhenFailedInput_ReturnsError()
//    {
//        var result = TestService.DoNothing("  ");

//        result.Should().BeEquivalentTo(new Wrong("Input value can't be empty"));
//    }

//    [Fact]
//    public void WhenValidInput_ThenNextMethodInvoked()
//    {
//        string s = "cba";
//        var result = TestService.DoNothing("abc")
//            .Then(_ => TestService.ToUpperCase(s));

//        result.Should().BeEquivalentTo(new Right<string>("CBA"));
//    }

//    [Fact]
//    public void WhenError_ThenProperlyPassesIt()
//    {
//        string s = "cba";
//        var result = TestService.DoNothing("    ")
//            .Then(_ => TestService.ToUpperCase(s));

//        result.Should().BeEquivalentTo(new Wrong("Input value can't be empty"));
//    }
//}