namespace NoIf.Tests;

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
