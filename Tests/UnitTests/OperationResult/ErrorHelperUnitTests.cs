using VIAEventsAssociation.Core.Tools.OperationResult.Error;
using VIAEventsAssociation.Core.Tools.OperationResult.Helpers;

namespace UnitTests.OperationResult;

public class ErrorHelperUnitTests
{
    [Fact]
    public void CreateVeaError_WithMessageAndErrorType_ShouldCreateCorrectVeaError()
    {
        var errorMessage = "This is an error message.";
        var errorType = ErrorType.Unknown;
        var veaError = ErrorHelper.CreateVeaError(errorMessage, errorType);
        Assert.Equal(errorType, veaError.Type);
        Assert.Equal($"Error of type {errorType.DisplayName}: {errorMessage}", veaError.Message.Message);
    }
    
    [Fact]
    public void CreateVeaError_WithErrorMessageAndErrorType_ShouldCreateCorrectVeaError()
    {
        var errorMessage = new ErrorMessage("Custom error message.");
        var errorType = ErrorType.ValidationFailed;
        var veaError = ErrorHelper.CreateVeaError(errorMessage, errorType);
        Assert.Equal(errorType, veaError.Type);
        Assert.Equal(errorMessage, veaError.Message);
    }
}