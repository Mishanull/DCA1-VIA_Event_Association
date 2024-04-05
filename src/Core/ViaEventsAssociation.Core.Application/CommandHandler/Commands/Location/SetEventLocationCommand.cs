using ViaEventAssociation.Core.Domain.Common.Base;
using ViaEventAssociation.Core.Domain.EventAgg;
using ViaEventAssociation.Core.Domain.LocationAgg;
using ViaEventsAssociation.Core.Application.CommandHandler.Common.Base;
using VIAEventsAssociation.Core.Tools.OperationResult.OperationResult;

namespace ViaEventsAssociation.Core.Application.CommandHandler.Commands.Location;

public class SetEventLocationCommand : Command
{
    internal VeaEventId EventId { get; private init; }
    internal LocationId LocationId { get; private init; }

    public static Result<SetEventLocationCommand> Create(string eventId, string locationId)
    {
        var eventIdResult = TId.FromString<VeaEventId>(eventId);
        var locationIdResult = TId.FromString<LocationId>(locationId);
        var result = new Result<SetEventLocationCommand>(null);
        result.CollectFromMultiple(eventIdResult, locationIdResult);
        if (result.IsErrorResult())
        {
            return result;
        }

        return new Result<SetEventLocationCommand>(new SetEventLocationCommand()
        {
            EventId = eventIdResult.Value!,
            LocationId = locationIdResult.Value!
        });
    }
    
    protected override IEnumerable<object?> GetEqualityComponents()
    {
        yield return EventId;
        yield return LocationId;
    }
}