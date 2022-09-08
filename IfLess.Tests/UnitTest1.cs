namespace IfLess.Tests;

public class BasicFlowTests
{
    [Fact]
    public void WhenValidInput_ReturnsValidResult()
    {
        var result = TestService.ReverseString("abc");

        result.Should().BeOfType<Valid<string>>();
    }

    [Fact]
    public void WhenFailedInput_ReturnsError()
    {
        var result = TestService.ReverseString("  ");

        result.Should().BeOfType<Error>();
    }

    [Fact]
    public void WhenError_ThenProperlyPassesIt()
    {
        var result = TestService.ReverseString("abc")
            .Then<string>(s => TestService.ToUpperCase(s));

        result.Should().BeOfType<Valid<string>>();
    }
}

class TestService
{
    public static Result ReverseString(string input)
    {
        if (string.IsNullOrWhiteSpace(input))
        {
            return new Error("Input value can't be empty");
        }

        var reversed = new string(input.Reverse().ToArray());
        return new Valid<string>(reversed);
    }

    public static Result ToUpperCase(string input)
    {
        if (string.IsNullOrWhiteSpace(input))
        {
            return new Error("Input value can't be empty");
        }

        return new Valid<string>(input.ToUpperInvariant());
    }
}