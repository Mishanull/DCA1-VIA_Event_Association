using ViaEventAssociation.Core.Domain.Common.Base;
using ViaEventAssociation.Core.Domain.CreatorAgg;
using ViaEventAssociation.Core.Domain.EventAgg;
using ViaEventAssociation.Core.Domain.GuestAgg.Guest;
using ViaEventsAssociation.Core.Application.CommandHandler.Common.Base;
using VIAEventsAssociation.Core.Tools.OperationResult.OperationResult;

namespace ViaEventsAssociation.Core.Application.CommandHandler.Commands.Guest;

public class GuestIsInvitedToEventCommand : Command
{
    internal CreatorId CreatorId { get; private set; }
    internal GuestId GuestId { get; private set; }
    internal VeaEventId EventId { get; private set; }

    public static Result<GuestIsInvitedToEventCommand> Create(string creatorId, string guestId, string veaEventId)
    {
        Result<GuestIsInvitedToEventCommand> result = new Result<GuestIsInvitedToEventCommand>(null);
        var cIdResult = TId.FromString<CreatorId>(creatorId);
        var guestIdResult = TId.FromString<GuestId>(guestId);
        var eventIdResult = TId.FromString<VeaEventId>(veaEventId);
        result.CollectFromMultiple(cIdResult, guestIdResult, eventIdResult);
        if (result.IsErrorResult())
        {
            return result;
        }

        return new Result<GuestIsInvitedToEventCommand>(new GuestIsInvitedToEventCommand()
        {
            CreatorId = cIdResult.Value!,
            GuestId = guestIdResult.Value!,
            EventId = eventIdResult.Value!
        });
    }

    protected override IEnumerable<object?> GetEqualityComponents()
    {
        yield return CreatorId;
        yield return GuestId;
        yield return EventId;
    }
}