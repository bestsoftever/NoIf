using Xunit.Sdk;

namespace NoIf.Tests;

// TODO: probably can be removed
public class HandleErrorsTests
{
    class Error1 : Error
    {
        public Error1() : base("Error 1")
        {
        }
    }

    class Error2 : Error
    {
        public Error2() : base("Error 2")
        {
        }
    }

    public static Result<string> DoStuff(string input)
    {
        if (string.IsNullOrWhiteSpace(input))
        {
            return new Error("Input value can't be empty");
        }

        return new string(input.Reverse().ToArray());
    }

    [Fact]
    public void WhenErrorIsNotAnError_ReturnsSomethingElse()
    {
        static Result<string> ReturnError() => new Error1();

        string errorMessage = string.Empty;
        Result<string> result = ReturnError()
            .Swap<Error1>(e => "replace error1")
            .Swap<Error2>(e => "replace error2")
            .Then(s => TestService.ToUpperCase(s));

        result.Should().Be("REPLACE ERROR1");
    }
}

// TODO: probably remove error test cases, leave only swaping a valid data
public class SwapTests
{
    class NotAnError : Error
    {
        public NotAnError() : base("Not really an error")
        {
        }
    }

    // class Error2 : Error
    // {
    //     public Error2() : base("Error 2")
    //     {
    //     }
    // }

    public static Result<string> DoStuff(string input)
    {
        if (string.IsNullOrWhiteSpace(input))
        {
            return new Error("Input value can't be empty");
        }

        return new string(input.Reverse().ToArray());
    }

    [Fact]
    public void WhenErrorIsNotAnError_ReturnsValidValue()
    {
        static Result<string> ReturnError() => new NotAnError();

        Result<string> result = ReturnError()
            .Swap<NotAnError>(e => "something else")
            .Then(s => TestService.ToUpperCase(s));

        result.Should().Be("SOMETHING ELSE");
    }

    [Fact]
    public void WhenErrorIsNotAnError_ReturnsValidValue2()
    {
        static Result<string> ReturnError() => "abc";

        Result<string> result = ReturnError()
            .Swap<NotAnError>(e => "something else")
            .Then(s => TestService.ToUpperCase(s));

        result.Should().Be("ABC");
    }

    [Fact]
    public void WhenValidValueIsAnError_ReturnsError()
    {
        static Result<string> ReturnValue() => "abc";

        Result<string> result = ReturnValue()
            .Swap<string>(e => new Error("weird"))
            .Then(s => TestService.ToUpperCase(s));

        result.Should().Be(new Error("weird"));
    }

    [Fact]
    public void WhenValidValueIsAnError_ReturnsError2()
    {
        static Result<string> ReturnValue() => new Error("abc");

        Result<string> result = ReturnValue()
            .Swap<string>(e => new Error("weird"))
            .Then(s => TestService.ToUpperCase(s));

        result.Should().Be(new Error("abc"));
    }


    //[Fact]
    //public void WhenValidValueIsInFactError_ReturnsError()
    //{
    //    static Result<string> ReturnValue() => "VALUE";

    //    Result<string> result = ReturnValue()
    //        .Swap<NotAnError>(e => "something else")
    //        .Then(s => TestService.ToUpperCase(s));

    //    result.Should().Be("SOMETHING ELSE");
    //}
}


public class IntegrationTests
{
    // The stupidest class hierarchy possible
    public abstract class Animal(string name) { public string Name { get; init; } = name; }
    public sealed class Dog(string name) : Animal(name) { }
    public sealed class Cat(string name) : Animal(name) { }
    public sealed class ThisParrotIsDeadError : Error
    {
        public ThisParrotIsDeadError() : base(":(")
        {
        }
    }

    public static TheoryData<Result<Animal>, string, string, bool> Data => new()
    {
        { new Dog("piesek"), "CAT IS PIESEK", string.Empty, true },
        { new ThisParrotIsDeadError(), "CAT IS KOTEK", ":(", false }
    };

    [Theory, MemberData(nameof(Data))]
    public void Full_Flow_Works(Result<Animal> oracleResult, string expectedResult, string expectedError, bool expectedPies)
    {
        Result<Animal> GetRandomPetFromOracle() => oracleResult;
        string errorMessage = string.Empty;
        bool isPies = false;

        var result = GetRandomPetFromOracle()
            .Act<Error>(e => errorMessage = e.Message)
            .Act<Dog>(d => isPies = true)
            .Swap<ThisParrotIsDeadError>(e => new Cat("kotek"))
            .Swap<Dog>(d => new Cat(d.Name))
            .Then(c => TestService.ToUpperCase($"{c.GetType().Name} is {c.Name}"));

        result.Should().Be(expectedResult);
        errorMessage.Should().Be(expectedError);
        isPies.Should().Be(expectedPies);
    }
}

public class NestedThenTests
{
    [Fact]
    public void NestedFlow_Works()
    {
        static Result<int> GetRandomNumber() => 4;
        static Result<int> GetBiggerRandomNumber() => 5;

        var result = GetRandomNumber()
            .Then(x =>
            {
                return GetBiggerRandomNumber()
                    .Then<int>(y => x * y);
            });

        result.Should().Be(20);
    }
}

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

public class NoneEqualityTests
{
    [Fact]
    public void NoneIsNone()
    {
        Result.None.Should().Be(Result.None);
    }
}

public class ErrorEqualityTests
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