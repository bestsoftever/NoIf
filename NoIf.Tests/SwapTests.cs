namespace NoIf.Tests;

public class SwapTests
{
    [Fact]
    public void SyncToSync_Valid()
    {
        Result<string> result = TestService.ReverseString("abc")
            .Swap<string>(e => new Error("weird"))
            .Then(s => TestService.ToUpperCase(s));

        result.Should().Be(new Error("weird"));
    }

    [Fact]
    public void SyncToSync_Error()
    {
        Result<string> result = TestService.ReverseString("  ")
            .Swap<string>(e => new Error("weird"))
            .Then(s => TestService.ToUpperCase(s));

        result.Should().Be(new Error(TestService.ErrorMessage));
    }
}
