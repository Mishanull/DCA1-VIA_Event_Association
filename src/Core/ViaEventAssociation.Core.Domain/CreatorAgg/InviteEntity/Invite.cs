using ViaEventAssociation.Core.Domain.Common.Base;
using ViaEventAssociation.Core.Domain.EventAgg;
using ViaEventAssociation.Core.Domain.GuestAgg.Guest;
using ViaEventAssociation.Core.Domain.GuestAgg.Request;
using ViaEventAssociation.Core.Domain.GuestAgg.RequestEntity;
using VIAEventsAssociation.Core.Tools.OperationResult.Helpers;
using VIAEventsAssociation.Core.Tools.OperationResult.OperationResult;

namespace ViaEventAssociation.Core.Domain.CreatorAgg.InviteEntity;

public class InviteId : TId;
public class Invite : Entity<InviteId>
{
    internal InviteStatus InviteStatus {get; private set; } = InviteStatus.Pending;
    internal DateTime Timestamp { get; private set; } = DateTime.Now;
    internal CreatorId CreatorId { get; private set; }
    internal GuestId GuestId { get; private set; }
    internal VeaEventId EventId { get; private set; }
    protected Invite(InviteId id, CreatorId creatorId, GuestId guestId, VeaEventId veaEventId) : base(id)
    {
        CreatorId = creatorId;
        GuestId = guestId;
        EventId = veaEventId;
        Id = id;
    }
    private Invite(){} //EF Core

    public static Result<Invite> Create(GuestId guestId, CreatorId creatorId, VeaEventId veaEventId)
    {
        return ResultHelper.CreateSuccess(new Invite(new InviteId(),creatorId, guestId,veaEventId));
    }
    
    public void Accept()
    {
        InviteStatus = InviteStatus.Accepted;
    }

    public void Decline()
    {
        InviteStatus = InviteStatus.Declined;
    }
}