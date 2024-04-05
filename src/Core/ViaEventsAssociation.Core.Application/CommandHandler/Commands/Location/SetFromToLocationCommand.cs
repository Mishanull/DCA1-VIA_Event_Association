using ViaEventAssociation.Core.Domain.Common.Base;
using ViaEventAssociation.Core.Domain.Common.ValueObjects;
using ViaEventAssociation.Core.Domain.LocationAgg;
using ViaEventsAssociation.Core.Application.CommandHandler.Common.Base;
using VIAEventsAssociation.Core.Tools.OperationResult.OperationResult;

namespace ViaEventsAssociation.Core.Application.CommandHandler.Commands.Location;

public class SetFromToLocationCommand : Command
{
    internal FromTo FromTo { get; private init; }
    internal LocationId LocationId { get; private init; }
    public static Result<SetFromToLocationCommand> Create(DateTime start, DateTime end, string locationId)
    {
        var result = new Result<SetFromToLocationCommand>(null);
        var fromToResult = FromTo.Create(start, end);
        var locationIdResult = TId.FromString<LocationId>(locationId);
        result.CollectFromMultiple(fromToResult, locationIdResult);
        if (result.IsErrorResult())
        {
            return result;
        }

        return new Result<SetFromToLocationCommand>(new SetFromToLocationCommand()
        {
            FromTo = fromToResult.Value!,
            LocationId = locationIdResult.Value!
        });
    }
    
    protected override IEnumerable<object?> GetEqualityComponents()
    {
        yield return FromTo;
    }
}