namespace VIAEventsAssociation.Core.Tools.OperationResult.Error;

public class ErrorType : Enumeration.Enumeration
{
    public static readonly ErrorType Unknown = new(0, "Unknown");
    public static readonly ErrorType ValidationFailed = new(100, "Validation Failed");
    public static readonly ErrorType ResourceNotFound = new(101, "Resource Not Found");
    public static readonly ErrorType Unauthorized = new(102, "Unauthorized");
    public static readonly ErrorType InvalidTitle = new(200, "Invalid Title");
    public static readonly ErrorType InvalidDescription = new(202, "Invalid Description");
    public static readonly ErrorType InvalidFromTo = new(203, "Invalid FromTo");
    public static readonly ErrorType InvalidMaxGuests = new(204, "Invalid MaxGuests");
    public static readonly ErrorType InvalidStatus = new(205, "InvalidStatus");
    public static readonly ErrorType ActionNotAllowed = new(300, "Action Not Allowed");
    
    public static readonly ErrorType EventIsFull = new(206, "EventIsFull");
    public static readonly ErrorType EventNotActive= new(207, "EventNotActive");
    public static readonly ErrorType EventHasEnded = new(208, "EventHasEnded");
    public static readonly ErrorType EventIsPrivate = new(209, "EventIsPrivate");
    public static readonly ErrorType AlreadyAParticipantInEvent = new(210, "AlreadyAParticipantInEvent");
    public static readonly ErrorType InviteNotPending = new(211, "InviteNotPending");
    
    private ErrorType(int code, string type) : base(code, type) {}
    
    // Public parameterless constructor
    public ErrorType() {}
}