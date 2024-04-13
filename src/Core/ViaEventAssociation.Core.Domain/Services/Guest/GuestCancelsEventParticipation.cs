using ViaEventAssociation.Core.Domain.EventAgg;
using ViaEventAssociation.Core.Domain.GuestAgg;
using ViaEventAssociation.Core.Domain.GuestAgg.Guest;
using ViaEventAssociation.Core.Domain.GuestAgg.Request;
using ViaEventAssociation.Core.Domain.GuestAgg.RequestEntity;
using VIAEventsAssociation.Core.Tools.OperationResult.Error;
using VIAEventsAssociation.Core.Tools.OperationResult.Helpers;
using VIAEventsAssociation.Core.Tools.OperationResult.OperationResult;

namespace ViaEventAssociation.Core.Domain.Services.Guest;

public class GuestCancelsEventParticipation(
    IGuestRepository guestRepo,
    IVeaEventRepository eventRepo
  )
{
    public async Task<Result> Handle(RequestId requestId)
    {
        var result = new Result();
        var findRequestResult = await guestRepo.FindRequestAsync(requestId);
        if (findRequestResult.IsErrorResult())
        {
            return findRequestResult;
        }
        var request = findRequestResult.Value!;
        var findGuestResult = await guestRepo.FindAsync(request.GuestId);
        var findEventResult = await eventRepo.FindAsync(request.EventId);
        result.CollectFromMultiple(findGuestResult, findEventResult);
        if (result.IsErrorResult())
        {
            return result;
        }

        VeaGuest guest = findGuestResult.Value!;
        VeaEvent veaEvent = findEventResult.Value!;

        if (ValidateEventHasNotEnded(veaEvent, result, out var result1)) return result1;

        CancelRequestAndUpdateAggregates(veaEvent, guest, request);

        return result;
    }

    private void CancelRequestAndUpdateAggregates(VeaEvent veaEvent, VeaGuest guest, Request request)
    {
        if (veaEvent.IsParticipant(guest.Id))
        {
            veaEvent.RemoveParticipant(guest.Id);
        }

        request.CancelRequest();
        guest.AddRequest(request);
    }

    private static bool ValidateEventHasNotEnded(VeaEvent veaEvent, Result result, out Result result1)
    {
        if (veaEvent.FromTo!.End.Date < DateTime.Now)
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
}