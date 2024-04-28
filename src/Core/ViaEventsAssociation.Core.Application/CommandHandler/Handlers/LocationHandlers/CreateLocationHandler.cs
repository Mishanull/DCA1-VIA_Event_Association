using ViaEventAssociation.Core.Domain.LocationAgg;
using ViaEventsAssociation.Core.Application.CommandHandler.Commands.Location;
using ViaEventsAssociation.Core.Application.CommandHandler.Common.Base;
using VIAEventsAssociation.Core.Tools.OperationResult.OperationResult;

namespace ViaEventsAssociation.Core.Application.CommandHandler.Handlers.LocationHandlers;

internal class CreateLocationHandler  : ICommandHandler<CreateLocationCommand>
{
    private readonly ILocationRepository _locationRepository;
    public CreateLocationHandler(ILocationRepository locationRepository)
    {
        _locationRepository = locationRepository;
    }
    
    public async Task<Result> HandleAsync(CreateLocationCommand command)
    {
        var locationCreationResult = Location.Create( command.LocationName,command.CreatorId);
        if (locationCreationResult.IsErrorResult())
        {
            return locationCreationResult;
        }

        var repositoryResult = await _locationRepository.AddAsync(locationCreationResult.Value!);
        if (repositoryResult.IsErrorResult())
        {
            return repositoryResult;
        }

        return new Result();
    }
}