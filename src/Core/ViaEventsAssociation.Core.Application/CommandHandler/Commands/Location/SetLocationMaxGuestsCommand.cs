using ViaEventAssociation.Core.Domain.Common.Base;
using ViaEventAssociation.Core.Domain.Common.ValueObjects;
using ViaEventAssociation.Core.Domain.EventAgg;
using ViaEventAssociation.Core.Domain.LocationAgg;
using ViaEventsAssociation.Core.Application.CommandHandler.Common.Base;
using VIAEventsAssociation.Core.Tools.OperationResult.OperationResult;

namespace ViaEventsAssociation.Core.Application.CommandHandler.Commands.Location;

public class SetLocationMaxGuestsCommand : Command
{
    internal MaxGuests MaxGuests { get; private init; }
    internal LocationId LocationId { get; private init; }

    public static Result<SetLocationMaxGuestsCommand> Create(int maxGuests, string locationId)
    {
        var maxGuestsResult = MaxGuests.Create(maxGuests);
        var result = new Result<SetLocationMaxGuestsCommand>(null);
        var locationIdResult = TId.FromString<LocationId>(locationId);
        result.CollectFromMultiple(maxGuestsResult, locationIdResult);
        if (result.IsErrorResult())
        {
            return result;
        }

        return new Result<SetLocationMaxGuestsCommand>(new SetLocationMaxGuestsCommand()
        {
            MaxGuests = maxGuestsResult.Value!,
            LocationId = locationIdResult.Value!
        });
    }
    protected override IEnumerable<object?> GetEqualityComponents()
    {
        yield return MaxGuests;
    }
}