using ViaEventAssociation.Core.Domain.LocationAgg;
using ViaEventsAssociation.Core.Application.CommandHandler.Commands.Location;
using ViaEventsAssociation.Core.Application.CommandHandler.Common.Base;
using VIAEventsAssociation.Core.Tools.OperationResult.OperationResult;

namespace ViaEventsAssociation.Core.Application.CommandHandler.Handlers.LocationHandlers;

public class SetFromToLocationHandler : ICommandHandler<SetFromToLocationCommand>
{

    private readonly ILocationRepository _locationRepository;
    internal SetFromToLocationHandler(ILocationRepository locationRepository)
    {
        _locationRepository = locationRepository;
    }
    public async Task<Result> HandleAsync(SetFromToLocationCommand command)
    {
        var repositoryResult = await _locationRepository.FindAsync(command.LocationId);
        if (repositoryResult.IsErrorResult())
        {
            return repositoryResult;
        }

        var location = repositoryResult.Value!;
        location.SetFromTo(command.FromTo);
        return new Result();
    }
}