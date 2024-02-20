namespace VIAEventsAssociation.Core.Tools.OperationResult.Error;

public class ErrorMessage
{
    public string Message { get; }

    public ErrorMessage(string message)
    {
        Message = message;
    }
}