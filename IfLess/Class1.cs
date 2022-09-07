namespace IfLess;

public abstract class Result
{

}

public class Valid : Result
{

}

public abstract class ErrorBase : Result
{

}

public class Error : ErrorBase
{
    public string Message { get; init; }

    public Error(string message)
    {
        Message = message;
    }
}