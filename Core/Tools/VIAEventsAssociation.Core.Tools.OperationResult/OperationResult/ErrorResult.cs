using VIAEventsAssociation.Core.Tools.OperationResult.Error;

namespace VIAEventsAssociation.Core.Tools.OperationResult.OperationResult;

public class ErrorResult : Result<HashSet<VeaError>>
{
    public ErrorResult(HashSet<VeaError> errorList) : base(errorList) {}
    public ErrorResult() : base(new HashSet<VeaError>()) {}
    public void CollectError(VeaError error) => Value.Add(error);
}
