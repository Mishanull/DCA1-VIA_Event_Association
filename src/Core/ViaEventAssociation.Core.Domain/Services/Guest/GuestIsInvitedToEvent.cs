using ViaEventAssociation.Core.Domain.Contracts.Repositories;
using ViaEventAssociation.Core.Domain.CreatorAgg;
using ViaEventAssociation.Core.Domain.CreatorAgg.InviteEntity;
using ViaEventAssociation.Core.Domain.EventAgg;
using VIAEventsAssociation.Core.Tools.OperationResult.Error;
using VIAEventsAssociation.Core.Tools.OperationResult.Helpers;
using VIAEventsAssociation.Core.Tools.OperationResult.OperationResult;

namespace ViaEventAssociation.Core.Domain.Services.Guest;

public class GuestIsInvitedToEvent(
    IGuestRepository guestRepo,
    ICreatorRepository creatorRepo,
    IVeaEventRepository eventRepo,
    IInviteRepository inviteRepo)
{
    public async Task<Result> Handle(Invite invite) 
    {
        var result = new Result();
        if (ValidateGuestCreatorAndEventExistence(invite, result, out var findCreatorResult, out var findEventResult, out var immediateResult)) return immediateResult;
        if (ValidateEvent(findEventResult, result)) return result;
        await UpdateAggregates(invite, findCreatorResult, result);
        return result;
    }

    private async Task UpdateAggregates(Invite invite, Result<Creator> findCreatorResult, Result result)
    {
        var creator = findCreatorResult.Value!;
        creator.AddInvite(invite);
        await inviteRepo.AddAsync(invite); 
        await creatorRepo.UpdateAsync(creator);
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
                return true;
            }
        }

        if (veaEvent.IsFull())
        {
            result.CollectError(ErrorHelper.CreateVeaError("Event is full, cannot invite more people.",
                ErrorType.EventIsFull));
            {
                return true;
            }
        }

        return false;
    }

    private bool ValidateGuestCreatorAndEventExistence(Invite invite, Result result,
        out Result<Creator> findCreatorResult,
        out Result<VeaEvent> findEventResult, out Result result1)
    {
        var findGuestResult = guestRepo.Find(invite.GuestId);
        findCreatorResult = creatorRepo.Find(invite.CreatorId);
        findEventResult = eventRepo.Find(invite.EventId);
        result.CollectErrors(findGuestResult.Errors);
        result.CollectErrors(findCreatorResult.Errors);
        result.CollectErrors(findEventResult.Errors);
        if (result.IsErrorResult())
        {
            {
                result1 = result;
                return true;
            }
        }

        result1 = result;
        return false;
    }
}