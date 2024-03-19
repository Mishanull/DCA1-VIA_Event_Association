using VIAEventsAssociation.Core.Tools.OperationResult.Error;
using VIAEventsAssociation.Core.Tools.OperationResult.OperationResult;

namespace VIAEventsAssociation.Core.Tools.OperationResult.Helpers;

public static class ResultHelper
{
    public static Result<T> CreateSuccess<T>(T value) => new(value);

    public static Result CreateSuccess() => new();


    public static Result CreateErrorResultWithSingleError(ErrorType errorType, ErrorMessage errorMessage)
    {
        var errorResult = new Result();
        errorResult.CollectError(new VeaError(errorType, errorMessage));
        return errorResult;
    }

    public static Result CreateErrorResultWithSingleError(VeaError error)
    {
        var errorResult = new Result();
        errorResult.CollectError(error);
        return errorResult;
    }

    public static Result CreateErrorResultWithMultipleErrors(IEnumerable<VeaError> errors)
    {
        var errorResult = new Result();
        errorResult.SetErrors(errors);
        return errorResult;
    }
}