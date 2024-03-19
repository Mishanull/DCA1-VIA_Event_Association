using VIAEventsAssociation.Core.Tools.Enumeration;

namespace ViaEventAssociation.Core.Domain.CreatorAgg.InviteEntity;

public class InviteStatus : Enumeration
{
    public static readonly InviteStatus Pending = new InviteStatus(1, "Pending");
    public static readonly InviteStatus Accepted = new InviteStatus(2, "Accepted");
    public static readonly InviteStatus Declined = new InviteStatus(3, "Declined");
    protected InviteStatus(int code, string status) : base(code, status)
    {
        
    }
}