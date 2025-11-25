namespace Inventory.Models;

public class Result
{
    public bool IsSuccess { get; }
    public string Error { get; }

    private Result(bool success, string error)
    {
        IsSuccess = success;
        Error = error;
    }

    public static Result Success()
         => new Result(true, string.Empty);

    public static Result Failure(string error)
        => new Result(false, error);
}