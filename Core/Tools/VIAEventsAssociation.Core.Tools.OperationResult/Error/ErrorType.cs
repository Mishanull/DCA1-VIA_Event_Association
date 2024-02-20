namespace VIAEventsAssociation.Core.Tools.OperationResult.Error;

public class ErrorType : Enumeration.Enumeration
{
    public static readonly ErrorType UnknownType = new(0, "Unknown");
    public static readonly ErrorType ValidationFailedType = new(100, "Validation Failed");
    public static readonly ErrorType ResourceNotFoundType = new(101, "Resource Not Found");
    public static readonly ErrorType UnauthorizedType = new(102, "Unauthorized");
    private ErrorType(int code, string type) : base(code, type) {}
    
    // Public parameterless constructor
    public ErrorType() {}
}