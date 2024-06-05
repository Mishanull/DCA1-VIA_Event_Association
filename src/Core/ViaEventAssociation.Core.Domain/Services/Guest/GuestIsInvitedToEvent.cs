using ViaEventAssociation.Core.Domain.CreatorAgg;
using ViaEventAssociation.Core.Domain.CreatorAgg.InviteEntity;
using ViaEventAssociation.Core.Domain.EventAgg;
using ViaEventAssociation.Core.Domain.GuestAgg;
using ViaEventAssociation.Core.Domain.GuestAgg.Guest;
using VIAEventsAssociation.Core.Tools.OperationResult.Error;
using VIAEventsAssociation.Core.Tools.OperationResult.Helpers;
using VIAEventsAssociation.Core.Tools.OperationResult.OperationResult;

namespace ViaEventAssociation.Core.Domain.Services.Guest;

public class GuestIsInvitedToEvent(
    IGuestRepository guestRepo,
    ICreatorRepository creatorRepo,
    IVeaEventRepository eventRepo)
{
    public async Task<Result> Handle(Invite invite)
    {
        var findGuestResult = await guestRepo.FindAsync(invite.GuestId);
        var findCreatorResult = await creatorRepo.FindAsync(invite.CreatorId);
        var findEventResult = await eventRepo.FindAsync(invite.EventId);
        var result = new Result();
        result.CollectFromMultiple(findEventResult, findGuestResult, findCreatorResult);
        if (result.IsErrorResult())
        {
            return result;
        }
        
        if (!ValidateEvent(findEventResult, result)) return result;
        UpdateAggregates(invite, findCreatorResult);
        return result;
    }

    private void UpdateAggregates(Invite invite, Result<Creator> findCreatorResult)
    {
        var creator = findCreatorResult.Value!;
        creator.AddInvite(invite);
    }

    private static bool ValidateEvent(Result<VeaEvent> findEventResult, Result result)
    {
        var veaEvent = findEventResult.Value!;
        if (veaEvent.VeaEventStatus.Equals(VeaEventStatus.Cancelled) ||
            veaEvent.VeaEventStatus.Equals(VeaEventStatus.Draft))
        {
            result.CollectError(ErrorHelper.CreateVeaError("An invite can only be sent for an active or ready event.",
                ErrorType.EventNotActive));
            {
                return false;
            }
        }

        if (veaEvent.IsFull())
        {
            result.CollectError(ErrorHelper.CreateVeaError("Event is full, cannot invite more people.",
                ErrorType.EventIsFull));
            {
                return false;
            }
        }

        return true;
    }
}