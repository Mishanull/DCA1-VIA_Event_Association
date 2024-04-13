using ViaEventAssociation.Core.Domain.LocationAgg;
using ViaEventsAssociation.Core.Application.CommandHandler.Commands.Location;
using ViaEventsAssociation.Core.Application.CommandHandler.Common.Base;
using VIAEventsAssociation.Core.Tools.OperationResult.OperationResult;

namespace ViaEventsAssociation.Core.Application.CommandHandler.Handlers.LocationHandlers;

public class UpdateLocationNameHandler: ICommandHandler<UpdateLocationNameCommand>
{
    private readonly ILocationRepository _locationRepository;
    internal UpdateLocationNameHandler(ILocationRepository locationRepository)
    {
        _locationRepository = locationRepository;
    }
    
    public async Task<Result> HandleAsync(UpdateLocationNameCommand command)
    {
        var repositoryResult = await _locationRepository.FindAsync(command.LocationId);
        if (repositoryResult.IsErrorResult())
        {
            return repositoryResult;
        }

        var location = repositoryResult.Value!;
        location.UpdateName(command.LocationName);
        return new Result();
    }
}