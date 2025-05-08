namespace DineMetrics.Core.Shared;

public class ServiceResult
{
    public Error Error { get; init; }

    public bool IsSuccess => Error == null;
    
    internal ServiceResult()
    {
    }
    
    internal ServiceResult(Error error)
    {
        Error = error;
    }

    public static ServiceResult Success => new();
    
    public static ServiceResult Failure(Error error) => new(error);
    
    public static ServiceResult NotFound() => new(new Error("Not found"));
}

public class ServiceResult<TValue> : ServiceResult
{
    public TValue Value { get; }

    internal ServiceResult(TValue value) : base()
    {
        Value = value;
    }
        
    internal ServiceResult(Error error)
    {
        Error = error;
    }

    public static new ServiceResult<TValue> Success(TValue value) => new(value);
        
    public static new ServiceResult<TValue> Failure(Error error) => new(error);
    
    public static ServiceResult<TValue> NotFound() => new(new Error("Not found"));
}