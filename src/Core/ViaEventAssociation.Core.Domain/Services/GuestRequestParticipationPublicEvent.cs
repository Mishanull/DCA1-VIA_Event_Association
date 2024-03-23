using ViaEventAssociation.Core.Domain.Contracts.Repositories;
using ViaEventAssociation.Core.Domain.EventAgg;
using ViaEventAssociation.Core.Domain.GuestAgg.Guest;
using ViaEventAssociation.Core.Domain.GuestAgg.RequestEntity;
using VIAEventsAssociation.Core.Tools.OperationResult.Error;
using VIAEventsAssociation.Core.Tools.OperationResult.Helpers;
using VIAEventsAssociation.Core.Tools.OperationResult.OperationResult;

namespace ViaEventAssociation.Core.Domain.Services;

public class GuestRequestParticipationPublicEvent(
    IGuestRepository guestRepo,
    IVeaEventRepository eventRepo,
    IRequestRepository requestRepo)
{
    public Result Handle(Request request)
    {
        if (Result(request, out var result, out var findGuestResult, out var findEventResult)) return result;
        var veaEvent = ValidateEvent(findEventResult, result);
        var guest = ValidateGuestParticipation(findGuestResult, veaEvent, result);
        if (HandleErrorResult(request, result, out var result1)) return result1;

        UpdateAggregates(request, veaEvent, guest, result);
        return result;
    }

    private void UpdateAggregates(Request request, VeaEvent veaEvent, VeaGuest guest, Result result)
    {
        veaEvent.AddParticipant(guest.Id);
        request.ApproveRequest();
        guest.AddRequest(request);
    }

    private static bool HandleErrorResult(Request request, Result result, out Result result1)
    {
        result1 = result;
        if (result.IsErrorResult())
        {
            request.DeclineRequest();
            {
                result1 = result;
                return true;
            }
        }

        return false;
    }

    private static VeaGuest ValidateGuestParticipation(Result<VeaGuest> findGuestResult, VeaEvent veaEvent,
        Result result)
    {
        VeaGuest guest = findGuestResult.Value;
        if (veaEvent.IsParticipant(guest.Id))
        {
            result.CollectError(ErrorHelper.CreateVeaError(
                "The guest is already a participant in the event.", ErrorType.AlreadyAParticipantInEvent));
        }

        return guest;
    }

    private static VeaEvent ValidateEvent(Result<VeaEvent> findEventResult, Result result)
    {
        VeaEvent veaEvent = findEventResult.Value;
        if (veaEvent.IsFull())
        {
            result.CollectError(ErrorHelper.CreateVeaError("Event is full.", ErrorType.EventIsFull));
        }

        if (veaEvent.FromTo.End.Date < DateTime.Now)
        {
            result.CollectError(ErrorHelper.CreateVeaError(
                "Event has already ended.", ErrorType.EventHasEnded));
        }

        if (Equals(veaEvent.VeaEventStatus, VeaEventStatus.Cancelled) ||
            Equals(veaEvent.VeaEventStatus, VeaEventStatus.Draft) ||
            Equals(veaEvent.VeaEventStatus, VeaEventStatus.Ready))
        {
            result.CollectError(ErrorHelper.CreateVeaError(
                "Event  is not active.", ErrorType.EventNotActive));
        }

        if (Equals(veaEvent.VeaEventType, VeaEventType.Private))
        {
            result.CollectError(ErrorHelper.CreateVeaError(
                "Event is private, only public events can be participated in.",
                ErrorType.EventIsPrivate));
        }

        return veaEvent;
    }

    private bool Result(Request request, out Result result, out Result<VeaGuest> findGuestResult,
        out Result<VeaEvent> findEventResult)
    {
        result = new Result();
        findGuestResult = guestRepo.Find(request.GuestId);
        findEventResult = eventRepo.Find(request.EventId);
        result.CollectErrors(findGuestResult.Errors);
        result.CollectErrors(findEventResult.Errors);
        if (result.IsErrorResult())
        {
            return true;
        }

        return false;
    }
}