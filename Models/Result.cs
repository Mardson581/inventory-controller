namespace Inventory.Models;

public class Result<TEntity>
{
    public bool IsSuccess { get; }
    public string Error { get; }
    public TEntity? Data { get; }

    private Result(bool success, string error, TEntity? data)
    {
        IsSuccess = success;
        Error = error;
        Data = data;
    }

    public static Result<TEntity?> Success(TEntity? data)
         => new Result<TEntity?>(true, string.Empty, data);

    public static Result<TEntity?> Failure(string error, TEntity? data)
        => new Result<TEntity?>(false, error, data);
}