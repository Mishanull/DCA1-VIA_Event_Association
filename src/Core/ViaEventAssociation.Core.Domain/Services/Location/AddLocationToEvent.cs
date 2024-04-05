using ViaEventAssociation.Core.Domain.Contracts.Repositories;
using ViaEventAssociation.Core.Domain.EventAgg;
using ViaEventAssociation.Core.Domain.LocationAgg;
using VIAEventsAssociation.Core.Tools.OperationResult.OperationResult;

namespace ViaEventAssociation.Core.Domain.Services.Location;

public class AddLocationToEvent(ILocationRepository locationRepository, IVeaEventRepository veaEventRepository)
{
    public async Task<Result> Handle(LocationId locationId, VeaEventId veaEventId)
    {
        var locationResult = await locationRepository.FindAsync(locationId);
        var veaEventResult = await veaEventRepository.FindAsync(veaEventId);
        var result = new Result();
        result.CollectFromMultiple(locationResult, veaEventResult);
        if (result.IsErrorResult())
        {
            return result;
        }

        var veaEvent = veaEventResult.Value!;
        veaEvent.SetLocationId(locationId);
        var updateResult = await veaEventRepository.UpdateAsync(veaEvent);
        return updateResult;
    }
}