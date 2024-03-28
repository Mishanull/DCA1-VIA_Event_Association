using ViaEventAssociation.Core.Domain.Common.Base;
using ViaEventAssociation.Core.Domain.EventAgg;
using ViaEventAssociation.Core.Domain.GuestAgg.Guest;
using ViaEventAssociation.Core.Domain.GuestAgg.Request;
using VIAEventsAssociation.Core.Tools.OperationResult.Error;
using VIAEventsAssociation.Core.Tools.OperationResult.Helpers;
using VIAEventsAssociation.Core.Tools.OperationResult.OperationResult;

namespace ViaEventAssociation.Core.Domain.GuestAgg.RequestEntity;

public class Request : Entity<RequestId>
{
    internal string Reason { get; private init; } = "";
    internal VeaEventId EventId { get; private init; }
    internal GuestId GuestId { get; private init; }

    internal RequestStatus RequestStatus { get; private set; } = RequestStatus.Pending;

    private Request(RequestId id, VeaEventId eventId, GuestId guestId) : base(id)
    {
        EventId = eventId;
        GuestId = guestId;
        Id = id;
    }

    private Request(RequestId id, VeaEventId eventId, GuestId guestId, string reason) : base(id)
    {
        EventId = eventId;
        GuestId = guestId;
        Id = id;
        Reason = reason;
    }

    public static Result<Request> Create(string reason, VeaEventId eventId, GuestId guestId)
    {
        var validationResult = new Result<Request>(null);
        validationResult.CollectErrors(ValidateReason(reason).Errors);
        if (validationResult.IsErrorResult())
        {
            return validationResult;
        } 
        
        var request = new Request(new RequestId(), eventId, guestId, reason);
        return new Result<Request>(request);
    }

    private static Result ValidateReason(string reason)
    {
        var result = new Result();
        if (reason.Length is < 10 or > 600)
        {
            result.CollectError(ErrorHelper.CreateVeaError("Reason should be between 10 and 600 characters long.", ErrorType.ValidationFailed));
        }

        return result;
    }

    public void CancelRequest()
    {
        RequestStatus = RequestStatus.Cancelled;
    }

    public void ApproveRequest()
    {
        RequestStatus = RequestStatus.Approved;
    }


    public void DeclineRequest()
    {
        RequestStatus = RequestStatus.Declined;
    }


    public void QueueRequest()
    {
        RequestStatus = RequestStatus.Queued;
    }
}