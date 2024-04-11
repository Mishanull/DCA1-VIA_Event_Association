using ViaEventAssociation.Core.Domain.Contracts.Repositories;
using ViaEventAssociation.Core.Domain.CreatorAgg;
using ViaEventAssociation.Core.Domain.CreatorAgg.InviteEntity;
using ViaEventAssociation.Core.Domain.EventAgg;
using ViaEventAssociation.Core.Domain.GuestAgg.Guest;
using VIAEventsAssociation.Core.Tools.OperationResult.Error;
using VIAEventsAssociation.Core.Tools.OperationResult.Helpers;
using VIAEventsAssociation.Core.Tools.OperationResult.OperationResult;

namespace ViaEventAssociation.Core.Domain.Services.Guest;

public class GuestAcceptsInvite(
    IGuestRepository guestRepo,
    ICreatorRepository creatorRepo,
    IVeaEventRepository eventRepo)

{
    public async Task<Result> Handle(InviteId inviteId)
    {
        var findInviteResult = await creatorRepo.FindInviteAsync(inviteId);
        if (findInviteResult.IsErrorResult())
        {
            return findInviteResult;
        }
        var invite = findInviteResult.Value!;

        var findCreatorResult = await creatorRepo.FindAsync(invite.CreatorId);
        var findGuestResult = await guestRepo.FindAsync(invite.GuestId);
        var findEventResult = await eventRepo.FindAsync(invite.EventId);
        var result = new Result();
        result.CollectFromMultiple(findGuestResult, findCreatorResult, findEventResult);
        if (result.IsErrorResult())
        {
            return result;
        }

        var creator = findCreatorResult.Value!;

        if (ValidateInvite(invite, result, out var result1)) return result1;

        if (ValidateVeaEvent(findEventResult, result, out var veaEvent, out var handle1)) return handle1;

        UpdateAggregates(findGuestResult, veaEvent, creator, invite);
        return result;
    }

    private void UpdateAggregates(Result<VeaGuest> findGuestResult, VeaEvent veaEvent, Creator creator, Invite invite)
    {
        var guest = findGuestResult.Value!;
        veaEvent.AddParticipant(guest.Id);
        invite.Accept();
        creator.AddInvite(invite);
    }

    private static bool ValidateVeaEvent(Result<VeaEvent> findEventResult, Result result, out VeaEvent veaEvent, out Result handle)
    {
        veaEvent = findEventResult.Value!;
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
        if (!invite.InviteStatus.Equals(InviteStatus.Pending))
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
}