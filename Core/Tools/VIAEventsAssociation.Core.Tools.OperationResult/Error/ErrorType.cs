namespace VIAEventsAssociation.Core.Tools.OperationResult.Error;

public class ErrorType : Enumeration.Enumeration
{
    public static readonly ErrorType UnknownType = new(0, "Unknown");
    public static readonly ErrorType ValidationFailedType = new(100, "Validation Failed");
    public static readonly ErrorType ResourceNotFoundType = new(101, "Resource Not Found");
    public static readonly ErrorType UnauthorizedType = new(102, "Unauthorized");
    public static readonly ErrorType InvalidTitle = new(200, "Invalid Title");
    public static readonly ErrorType InvalidDescription = new(202, "Invalid Description");
    public static readonly ErrorType InvalidFromTo = new(203, "Invalid FromTo");
    public static readonly ErrorType InvalidMaxGuests = new(204, "Invalid MaxGuests");
    public static readonly ErrorType ActionNotAllowed = new(300, "Action Not Allowed");
    
    private ErrorType(int code, string type) : base(code, type) {}
    
    // Public parameterless constructor
    public ErrorType() {}
}