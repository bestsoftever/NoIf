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

    static class TestService
    {
        public static Result<string> ReverseString(string input)
        {
            if (string.IsNullOrWhiteSpace(input))
            {
                return new Wrong("Input value can't be empty");
            }

            return new string(input.Reverse().ToArray());
        }

        public static Result<string> ToUpperCase(string input)
        {
            if (string.IsNullOrWhiteSpace(input))
            {
                return new Wrong("Input value can't be empty");
            }

            return input.ToUpperInvariant();
        }
    }
}