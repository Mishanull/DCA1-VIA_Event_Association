using VIAEventsAssociation.Core.Tools.OperationResult.Error;

namespace VIAEventsAssociation.Core.Tools.OperationResult.OperationResult;

public class Result
{
    public HashSet<VeaError> Errors { get; private set; } = [];
    
    public void CollectError(VeaError error) => Errors.Add(error); 
    
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