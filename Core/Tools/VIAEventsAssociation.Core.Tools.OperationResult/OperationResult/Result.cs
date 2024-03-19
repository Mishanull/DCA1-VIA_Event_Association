using VIAEventsAssociation.Core.Tools.OperationResult.Error;

namespace VIAEventsAssociation.Core.Tools.OperationResult.OperationResult;

public class Result
{
    public HashSet<VeaError> Errors { get; private set; } = [];

    public Result()
    {
    }

    public Result(HashSet<VeaError> errors)
    {
        Errors = errors;
    }

    public void CollectError(VeaError error) => Errors.Add(error);
    public void CollectErrors(HashSet<VeaError> errors) => Errors.UnionWith(errors);

    public bool IsErrorResult()
    {
        return Errors.Count > 0;
    }

    public void SetErrors(IEnumerable<VeaError> errors)
    {
        Errors = errors.ToHashSet();
    }
}

public class Result<T>(T value) : Result
{
    public T Value { get; } = value;
}