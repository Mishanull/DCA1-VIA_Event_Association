using ViaEventAssociation.Core.Domain.Contracts.Repositories;
using ViaEventAssociation.Core.Domain.CreatorAgg;
using ViaEventAssociation.Core.Domain.CreatorAgg.InviteEntity;
using ViaEventAssociation.Core.Domain.EventAgg;
using ViaEventAssociation.Core.Domain.GuestAgg.Guest;
using VIAEventsAssociation.Core.Tools.OperationResult.Error;
using VIAEventsAssociation.Core.Tools.OperationResult.Helpers;
using VIAEventsAssociation.Core.Tools.OperationResult.OperationResult;

namespace ViaEventAssociation.Core.Domain.Services.Guest;

public class GuestDeclinesInvite(
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
        if (ValidateVeaEvent(findEventResult, result, out var veaEvent, out var result1)) return result1;

        DeclineInviteAndUpdateAggregates(invite, creator, veaEvent, findGuestResult);
        return result;
    }

    private void DeclineInviteAndUpdateAggregates(Invite invite, Creator creator, VeaEvent veaEvent, Result<VeaGuest> findGuestResult)
    {
        invite.Decline();
        veaEvent.RemoveParticipant(findGuestResult.Value!.Id);
        creator.AddInvite(invite);
    }

    private static bool ValidateVeaEvent(Result<VeaEvent> findEventResult, Result result, out VeaEvent veaEvent,
        out Result result1)
    {
        veaEvent = findEventResult.Value!;
        result1 = result;
        if (veaEvent.VeaEventStatus.Equals(VeaEventStatus.Cancelled) ||
            veaEvent.VeaEventStatus.Equals(VeaEventStatus.Draft))
        {
            result.CollectError(ErrorHelper.CreateVeaError(
                "An invite cannot be be declined for a cancelled or draft event.",
                ErrorType.EventNotActive));
            {
                result1 = result;
                return true;
            }
        }

        if (veaEvent.VeaEventStatus.Equals(VeaEventStatus.Ready))
        {
            result.CollectError(ErrorHelper.CreateVeaError("This event cannot be declined yet.",
                ErrorType.EventNotActive));
            {
                result1 = result;
                return true;
            }
        }

        if (veaEvent.IsFull())
        {
            result.CollectError(ErrorHelper.CreateVeaError("Event is full, cannot invite more people.",
                ErrorType.EventIsFull));
            {
                result1 = result;
                return true;
            }
        }

        return false;
    }
}