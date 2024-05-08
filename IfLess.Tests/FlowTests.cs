using System.Reflection.Metadata.Ecma335;
using System.Reflection.PortableExecutable;

namespace IfLess.Tests;

public class ThenTests
{
    public static IEnumerable<object[]> ReturnsUpperCase()
    {
        yield return new object[] { "abc", "ABC" };
        yield return new object[] { "", new Error("Input value can't be empty") };
    }
    public static IEnumerable<object[]> ReturnsUpperCaseAndReverse()
    {
        yield return new object[] { "abc", "CBA" };
        yield return new object[] { "", new Error("Input value can't be empty") };
    }
    public static IEnumerable<object[]> ReturnsNone()
    {
        yield return new object[] { "abc", Result.None };
        yield return new object[] { "", new Error("Input value can't be empty") };
    }

    [Theory, MemberData(nameof(ReturnsUpperCaseAndReverse))]
    public void SyncToSync(string input, Result<string> expected)
    {
        Result<string> result = TestService.ReverseString(input)
            .Then(s => TestService.ToUpperCase(s));

        result.Should().Be(expected);
    }

    [Theory, MemberData(nameof(ReturnsUpperCaseAndReverse))]
    public async Task SyncToAsync(string input, Result<string> expected)
    {
        Result<string> result = await TestService.ReverseString(input)
            .Then(s => TestService.ToUpperCaseAsync(s));

        result.Should().Be(expected);
    }

    [Theory, MemberData(nameof(ReturnsNone))]
    public void SyncToNone(string input, Result<None> expected)
    {
        Result<None> result = TestService.ReverseString(input)
            .Then(s => TestService.DoNothing(s));

        result.Should().Be(expected);
    }

    [Theory, MemberData(nameof(ReturnsNone))]
    public async Task SyncToNoneAsync(string input, Result<None> expected)
    {
        Result<None> result = await TestService.ReverseString(input)
            .Then(s => TestService.DoNothingAsync(s));

        result.Should().Be(expected);
    }

    [Theory, MemberData(nameof(ReturnsUpperCaseAndReverse))]
    public async Task AsyncToSync(string input, Result<string> expected)
    {
        Result<string> result = await TestService.ReverseStringAsync(input)
            .Then(s => TestService.ToUpperCase(s));

        result.Should().Be(expected);
    }

    [Theory, MemberData(nameof(ReturnsUpperCaseAndReverse))]
    public async Task AsyncToAsync(string input, Result<string> expected)
    {
        Result<string> result = await TestService.ReverseStringAsync(input)
            .Then(s => TestService.ToUpperCaseAsync(s));

        result.Should().Be(expected);
    }

    [Theory, MemberData(nameof(ReturnsNone))]
    public async Task AsyncToNone(string input, Result<None> expected)
    {
        Result<None> result = await TestService.ReverseStringAsync(input)
            .Then(s => TestService.DoNothing(s));

        result.Should().Be(expected);
    }

    [Theory, MemberData(nameof(ReturnsNone))]
    public async Task AsyncToNoneAsync(string input, Result<None> expected)
    {
        Result<None> result = await TestService.ReverseStringAsync(input)
            .Then(s => TestService.DoNothingAsync(s));

        result.Should().Be(expected);
    }

    [Theory, MemberData(nameof(ReturnsUpperCase))]
    public void NoneToSync(string input, Result<string> expected)
    {
        Result<string> result = TestService.DoNothing(input)
            .Then(_ => TestService.ToUpperCase(input));

        result.Should().Be(expected);
    }

    [Theory, MemberData(nameof(ReturnsUpperCase))]
    public async Task NoneToAsync(string input, Result<string> expected)
    {
        Result<string> result = await TestService.DoNothing(input)
            .Then(_ => TestService.ToUpperCaseAsync(input));

        result.Should().Be(expected);
    }

    [Theory, MemberData(nameof(ReturnsNone))]
    public void NoneToNone(string input, Result<None> expected)
    {
        Result<None> result = TestService.DoNothing(input)
            .Then(_ => TestService.DoNothing(input));

        result.Should().Be(expected);
    }

    [Theory, MemberData(nameof(ReturnsNone))]
    public async Task NoneToNoneAsync(string input, Result<None> expected)
    {
        Result<None> result = await TestService.DoNothing(input)
            .Then(_ => TestService.DoNothingAsync(input));

        result.Should().Be(expected);
    }

    [Theory, MemberData(nameof(ReturnsUpperCase))]
    public async void NoneAsyncToSync(string input, Result<string> expected)
    {
        Result<string> result = await TestService.DoNothingAsync(input)
            .Then(_ => TestService.ToUpperCase(input));

        result.Should().Be(expected);
    }

    [Theory, MemberData(nameof(ReturnsUpperCase))]
    public async Task NoneAsyncToAsync(string input, Result<string> expected)
    {
        Result<string> result = await TestService.DoNothingAsync(input)
            .Then(_ => TestService.ToUpperCaseAsync(input));

        result.Should().Be(expected);
    }

    [Theory, MemberData(nameof(ReturnsNone))]
    public async void NoneAsyncToNone(string input, Result<None> expected)
    {
        Result<None> result = await TestService.DoNothingAsync(input)
            .Then(_ => TestService.DoNothing(input));

        result.Should().Be(expected);
    }

    [Theory, MemberData(nameof(ReturnsNone))]
    public async Task NoneAsyncToNoneAsync(string input, Result<None> expected)
    {
        Result<None> result = await TestService.DoNothingAsync(input)
            .Then(_ => TestService.DoNothingAsync(input));

        result.Should().Be(expected);
    }
}

public class NoneTests
{
    [Fact]
    public void NoneIsNone()
    {
        Result.None.Should().Be(Result.None);
    }
}

public class ErrorTests
{
    [Fact]
    public void SimpleErrorWorks()
    {
        var error = DoStuff();
        var result = error.Then<bool>(x => x.StartsWith("a"));

        result.Should().Be(new Error("wrong!"));

        static Result<string> DoStuff() => new Error("wrong!");
    }

    [Fact]
    public void ErrorWorks()
    {
        var error = DoStuff();
        var result = error.Then<bool>(x => x.StartsWith("a"));

        result.Should().Be(new Error("Some error"));

        static Result<string> DoStuff() => new Error("Some error");
    }

    [Fact]
    public void SameErrorsAreEqual()
    {
        new Error("Some error", new Error("Some inner error", new Error("Very inner error")), new Error("Another inner error"))
            .Should().Be(new Error("Some error", new Error("Some inner error", new Error("Very inner error")), new Error("Another inner error")));
    }

    [Fact]
    public void WhenErrorMessagesDiffers_ErrorsAreNotEqual()
    {
        new Error("Some error")
            .Should().NotBe(new Error("Some issue"));
    }

    [Fact]
    public void WhenOneErrorHasEmptyInnerErrors_ErrorsAreNotEqual()
    {
        new Error("Some error", new Error("Some inner error"), new Error("Another inner error"))
            .Should().NotBe(new Error("Some error"));
    }

    [Fact]
    public void WhenInnerErrorsHaveDifferentItemsCount_ErrorsAreNotEqual()
    {
        new Error("Some error", new Error("Some inner error"), new Error("Another inner error"), new Error("Additional error"))
            .Should().NotBe(new Error("Some error", new Error("Some inner error"), new Error("Another inner error")));
    }

    [Fact]
    public void WhenInnerErrorsHaveDifferentMessage_ErrorsAreNotEqual()
    {
        new Error("Some error", new Error("Some inner errr"), new Error("Another inner error"))
            .Should().NotBe(new Error("Some error", new Error("Some inner error"), new Error("Another inner error")));
    }
}