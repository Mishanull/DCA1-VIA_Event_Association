using ViaEventAssociation.Core.Domain.Contracts.Repositories;
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
    IVeaEventRepository eventRepo,
    IInviteRepository inviteRepo)
{
    public async Task<Result> Handle(InviteId inviteId)
    {
        if (ValidateInviteId(inviteId, out var findInviteResult)) return findInviteResult;

        if (ValidateCreatorGuestAndEventExistence(findInviteResult, out var result, out var invite,
                out var findGuestResult, out var findEventResult)) return result;

        if (ValidateVeaEvent(findEventResult, result, out var veaEvent, out var result1)) return result1;

        await DeclineInviteAndUpdateAggregates(invite, veaEvent, findGuestResult, result);
        return result;
    }

    private async Task DeclineInviteAndUpdateAggregates(Invite invite, VeaEvent veaEvent, Result<VeaGuest> findGuestResult,
        Result result)
    {
        invite.Decline();
        veaEvent.RemoveParticipant(findGuestResult.Value!.Id);
        await inviteRepo.UpdateAsync(invite);
        await eventRepo.UpdateAsync(veaEvent);
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

    private bool ValidateCreatorGuestAndEventExistence(Result<Invite> findInviteResult, out Result result,
        out Invite invite,
        out Result<VeaGuest> findGuestResult, out Result<VeaEvent> findEventResult)
    {
        result = new Result();
        invite = findInviteResult.Value!;
        findGuestResult = guestRepo.Find(invite.GuestId);
        var findCreatorResult = creatorRepo.Find(invite.CreatorId);
        findEventResult = eventRepo.Find(invite.EventId);
        result.CollectErrors(findGuestResult.Errors);
        result.CollectErrors(findCreatorResult.Errors);
        result.CollectErrors(findEventResult.Errors);
        result.CollectErrors(findInviteResult.Errors);

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