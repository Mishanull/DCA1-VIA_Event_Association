using ViaEventAssociation.Core.Domain.Common.Base;
using ViaEventAssociation.Core.Domain.LocationAgg;
using ViaEventsAssociation.Core.Application.CommandHandler.Common.Base;
using VIAEventsAssociation.Core.Tools.OperationResult.OperationResult;

namespace ViaEventsAssociation.Core.Application.CommandHandler.Commands.Location;

public class UpdateLocationNameCommand : Command
{
    internal LocationName LocationName { get; private init; }
    internal LocationId LocationId { get; private init; }
    
    public static Result<UpdateLocationNameCommand> Create(string locationName, string locationId)
    {
        var nameResult = LocationName.Create(locationName);
        var result = new Result<UpdateLocationNameCommand>(null);
        
        var locationIdResult = TId.FromString<LocationId>(locationId);
        result.CollectFromMultiple(nameResult, locationIdResult);

        if (result.IsErrorResult())
        {
            return result;
        }

        return new Result<UpdateLocationNameCommand>(new UpdateLocationNameCommand()
        {
            LocationName = nameResult.Value!,
            LocationId = locationIdResult.Value!
        });
    }

    protected override IEnumerable<object?> GetEqualityComponents()
    {
        yield return LocationName;
    }
}