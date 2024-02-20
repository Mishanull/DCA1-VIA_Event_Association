namespace VIAEventsAssociation.Core.Tools.OperationResult.OperationResult;

public class Result;

public class Result<T>(T value) : Result
{
    public T Value { get; } = value;
}