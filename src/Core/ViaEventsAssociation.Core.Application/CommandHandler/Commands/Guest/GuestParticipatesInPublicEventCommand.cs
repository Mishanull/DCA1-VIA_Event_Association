using ViaEventAssociation.Core.Domain.Common.Base;
using ViaEventAssociation.Core.Domain.EventAgg;
using ViaEventAssociation.Core.Domain.GuestAgg.Guest;
using ViaEventsAssociation.Core.Application.CommandHandler.Common.Base;
using VIAEventsAssociation.Core.Tools.OperationResult.OperationResult;

namespace ViaEventsAssociation.Core.Application.CommandHandler.Commands.Guest;

public class GuestParticipatesInPublicEventCommand : Command
{
    internal VeaEventId EventId { get; private init; }
    internal GuestId VeaGuestId { get; private init; }
    internal string Reason { get; private init; }

    public static Result<GuestParticipatesInPublicEventCommand> Create(string eventId, string guestId, string reason)
    {
        var eventIdCreationResult = TId.FromString<VeaEventId>(eventId);
        var guestIdCreationResult = TId.FromString<GuestId>(guestId);
        var result = new Result<GuestParticipatesInPublicEventCommand>(null);
        result.CollectFromMultiple(eventIdCreationResult, guestIdCreationResult);
        if (result.IsErrorResult())
        {
            return result;
        }

        return new Result<GuestParticipatesInPublicEventCommand>(new GuestParticipatesInPublicEventCommand()
        {
            EventId = eventIdCreationResult.Value!,
            VeaGuestId = guestIdCreationResult.Value!,
            Reason = reason
        });
    }

    protected override IEnumerable<object?> GetEqualityComponents()
    {
        yield return EventId;
        yield return VeaGuestId;
        yield return Reason;
    }
}