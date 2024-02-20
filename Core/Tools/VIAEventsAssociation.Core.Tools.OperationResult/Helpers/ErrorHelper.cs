using VIAEventsAssociation.Core.Tools.OperationResult.Error;

namespace VIAEventsAssociation.Core.Tools.OperationResult.Helpers;

public static class ErrorHelper
{
    public static VeaError CreateVeaError(string message, ErrorType errorType)
    {
        var errorMessage =
            new ErrorMessage($"Error of type {errorType.DisplayName}: {message}");
        return new VeaError(errorType, errorMessage);
    }

    public static VeaError CreateVeaError(ErrorMessage message, ErrorType errorType)
    {
        return new VeaError(errorType, message);
    }
}