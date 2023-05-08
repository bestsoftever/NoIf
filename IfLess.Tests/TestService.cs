namespace IfLess.Tests;

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

    public static async Task<Result<string>> ReverseStringAsync(string input)
    {
        await Task.Yield();

        if (string.IsNullOrWhiteSpace(input))
        {
            return new Wrong("Input value can't be empty");
        }

        return new string(input.Reverse().ToArray());
    }

    public static async Task<Result<string>> ToUpperCaseAsync(string input)
    {
        await Task.Yield();

        if (string.IsNullOrWhiteSpace(input))
        {
            return new Wrong("Input value can't be empty");
        }

        return input.ToUpperInvariant();
    }

    //public static Result DoNothing(string input)
    //{
    //    if (string.IsNullOrWhiteSpace(input))
    //    {
    //        return new Wrong("Input value can't be empty");
    //    }

    //    return Result.Empty;
    //}

    //public static async Task<Result> DoNothingAsync(string input)
    //{
    //    await Task.Yield();

    //    if (string.IsNullOrWhiteSpace(input))
    //    {
    //        return new Wrong("Input value can't be empty");
    //    }

    //    return Result.Empty;
    //}
}
