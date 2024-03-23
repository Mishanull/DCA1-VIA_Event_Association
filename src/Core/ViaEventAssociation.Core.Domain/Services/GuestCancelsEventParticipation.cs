using ViaEventAssociation.Core.Domain.Contracts.Repositories;
using ViaEventAssociation.Core.Domain.EventAgg;
using ViaEventAssociation.Core.Domain.GuestAgg.Guest;
using ViaEventAssociation.Core.Domain.GuestAgg.Request;
using ViaEventAssociation.Core.Domain.GuestAgg.RequestEntity;
using VIAEventsAssociation.Core.Tools.OperationResult.Error;
using VIAEventsAssociation.Core.Tools.OperationResult.Helpers;
using VIAEventsAssociation.Core.Tools.OperationResult.OperationResult;

namespace ViaEventAssociation.Core.Domain.Services;

public class GuestCancelsEventParticipation(
    IGuestRepository guestRepo,
    IVeaEventRepository eventRepo,
    IRequestRepository requestRepo)
{
    public Result Handle(RequestId requestId)
    {
        if (ValidateRequestId(requestId, out var findRequestResult)) return findRequestResult;

        if (ValidateGuestAndResultExist(findRequestResult, out var result, out var request, out var findGuestResult, out var findEventResult)) return result;

        VeaGuest guest = findGuestResult.Value;
        VeaEvent veaEvent = findEventResult.Value;

        if (ValidateEventHasNotEnded(veaEvent, result, out var result1)) return result1;

        CancelRequestAndUpdateAggregates(veaEvent, guest, result, request);

        return result;
    }

    private void CancelRequestAndUpdateAggregates(VeaEvent veaEvent, VeaGuest guest, Result result, Request request)
    {
        if (veaEvent.IsParticipant(guest.Id))
        {
            veaEvent.RemoveParticipant(guest.Id);
        }

        request.CancelRequest();
    }

    private static bool ValidateEventHasNotEnded(VeaEvent veaEvent, Result result, out Result result1)
    {
        if (veaEvent.FromTo.End.Date < DateTime.Now)
        {
            result.CollectError(ErrorHelper.CreateVeaError(
                "Event has already ended.", ErrorType.EventHasEnded));
            {
                result1 = result;
                return true;
            }
        }

        result1 = result;
        return false;
    }

    private bool ValidateGuestAndResultExist(Result<Request> findRequestResult, out Result result, out Request request,
        out Result<VeaGuest> findGuestResult, out Result<VeaEvent> findEventResult)
    {
        result = new Result();
        request = findRequestResult.Value;
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

    private bool ValidateRequestId(RequestId requestId, out Result<Request> findRequestResult)
    {
        findRequestResult = requestRepo.Find(requestId);
        if (findRequestResult.IsErrorResult())
        {
            return true;
        }

        return false;
    }
}