using VIAEventsAssociation.Core.Tools.Enumeration;

namespace ViaEventAssociation.Core.Domain.GuestAgg.Request;

public class RequestStatus : Enumeration
{
    public static readonly RequestStatus Pending= new (0, "Pending");
    public static readonly RequestStatus Approved = new (1, "Approved");
    public static readonly RequestStatus Cancelled = new (2, "Cancelled");
    public static readonly RequestStatus Declined = new (3, "Declined");
    public static readonly RequestStatus Queued = new (4, "Queued");
    
    private RequestStatus(int ordinal, string state ) : base(ordinal, state) {}
    
}