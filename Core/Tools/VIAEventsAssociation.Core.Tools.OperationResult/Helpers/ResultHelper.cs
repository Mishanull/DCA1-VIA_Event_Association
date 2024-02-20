using VIAEventsAssociation.Core.Tools.OperationResult.Error;
using VIAEventsAssociation.Core.Tools.OperationResult.OperationResult;

namespace VIAEventsAssociation.Core.Tools.OperationResult.Helpers;

public static class ResultHelper
{
    public static Result<T> CreateSuccess<T>(T value) => new(value);

    public static Result CreateSuccess() => new();
    public static ErrorResult CreateEmptyErrorResult() => new();

    public static ErrorResult CreateErrorResult(HashSet<VeaError> errors) => new(errors);

    public static ErrorResult CreateErrorResultWithSingleError(ErrorType errorType, ErrorMessage errorMessage)
    {
        var errorResult = new ErrorResult();
        errorResult.CollectError(new VeaError(errorType, errorMessage));
        return errorResult;
    }
}