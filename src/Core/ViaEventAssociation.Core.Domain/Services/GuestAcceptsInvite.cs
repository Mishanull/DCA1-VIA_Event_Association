using ViaEventAssociation.Core.Domain.Contracts;
using ViaEventAssociation.Core.Domain.CreatorAgg.InviteEntity;
using ViaEventAssociation.Core.Domain.EventAgg;
using ViaEventAssociation.Core.Domain.GuestAgg.Guest;
using VIAEventsAssociation.Core.Tools.OperationResult.Error;
using VIAEventsAssociation.Core.Tools.OperationResult.Helpers;
using VIAEventsAssociation.Core.Tools.OperationResult.OperationResult;

namespace ViaEventAssociation.Core.Domain.Services;

public class GuestAcceptsInvite(
    IGuestRepository guestRepo,
    ICreatorRepository creatorRepo,
    IVeaEventRepository eventRepo,
    IInviteRepository inviteRepo)
{
    public Result Handle(InviteId inviteId)
    {
        if (ValidateInviteId(inviteId, out var findInviteResult)) return findInviteResult;

        if (ValidateGuestCreatorAndEvent(findInviteResult, out var result, out var invite, out var findGuestResult, out var findEventResult)) return result;

        if (ValidateInvite(invite, result, out var result1)) return result1;
        
        if (ValidateVeaEvent(findEventResult, result, out var veaEvent, out var handle1)) return handle1;

        UpdateAggregates(findGuestResult, veaEvent, invite, result);
        return result;
    }

    private void UpdateAggregates(Result<VeaGuest> findGuestResult, VeaEvent veaEvent, Invite invite, Result result)
    {
        var guest = findGuestResult.Value;
        veaEvent.AddParticipant(guest.Id);
        eventRepo.Save(veaEvent);
        invite.Accept();
        result.CollectErrors(inviteRepo.Save(invite).Errors);
    }

    private static bool ValidateVeaEvent(Result<VeaEvent> findEventResult, Result result, out VeaEvent veaEvent, out Result handle)
    {
        veaEvent = findEventResult.Value;
        if (veaEvent.VeaEventStatus.Equals(VeaEventStatus.Cancelled) || veaEvent.VeaEventStatus.Equals(VeaEventStatus.Draft))
        {
            result.CollectError(ErrorHelper.CreateVeaError("An invite can only be accepted for an active event.",
                ErrorType.EventNotActive));
            {
                handle = result;
                return true;
            }
        }
        
        if (veaEvent.VeaEventStatus.Equals(VeaEventStatus.Ready))
        {
            result.CollectError(ErrorHelper.CreateVeaError("This event cannot be joined yet.",
                ErrorType.EventNotActive));
            {
                handle = result;
                return true;
            }
        }
        
        if (veaEvent.IsFull())
        {
            result.CollectError(ErrorHelper.CreateVeaError("Event is full, cannot invite more people.",
                ErrorType.EventIsFull));
            {
                handle = result;
                return true;
            }
        }

        handle = result;
        return false;
    }

    private static bool ValidateInvite(Invite invite, Result result, out Result result1)
    {
        if (!invite.InviteStatus.Equals( InviteStatus.Pending))
        {
            result.CollectError(ErrorHelper.CreateVeaError("Invite is not pending.",
                ErrorType.InviteNotPending));
            {
                result1 = result;
                return true;
            }
        }

        result1 = result; 
        return false;
    }

    private bool ValidateGuestCreatorAndEvent(Result<Invite> findInviteResult, out Result result, out Invite invite,
        out Result<VeaGuest> findGuestResult, out Result<VeaEvent> findEventResult)
    {
        result = new Result();
        invite = findInviteResult.Value;
        findGuestResult = guestRepo.Find(invite.GuestId);
        var findCreatorResult = creatorRepo.Find(invite.CreatorId);
        findEventResult = eventRepo.Find(invite.EventId);
        
        result.CollectErrors(findGuestResult.Errors);
        result.CollectErrors(findCreatorResult.Errors);
        result.CollectErrors(findEventResult.Errors);
        if (result.IsErrorResult())
        {
            return true;
        }

        return false;
    }

    private bool ValidateInviteId(InviteId inviteId, out Result<Invite> findInviteResult)
    {
        findInviteResult = inviteRepo.Find(inviteId);
        if (findInviteResult.IsErrorResult())
        {
            return true;
        }

        return false;
    }
}