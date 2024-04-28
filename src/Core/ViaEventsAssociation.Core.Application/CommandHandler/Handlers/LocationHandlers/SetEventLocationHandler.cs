using ViaEventAssociation.Core.Domain.EventAgg;
using ViaEventAssociation.Core.Domain.LocationAgg;
using ViaEventAssociation.Core.Domain.Services.Location;
using ViaEventsAssociation.Core.Application.CommandHandler.Commands.Location;
using ViaEventsAssociation.Core.Application.CommandHandler.Common.Base;
using VIAEventsAssociation.Core.Tools.OperationResult.OperationResult;

namespace ViaEventsAssociation.Core.Application.CommandHandler.Handlers.LocationHandlers;

internal class SetEventLocationHandler : ICommandHandler<SetEventLocationCommand>
{
    private readonly AddLocationToEvent _addLocationToEvent;
    public SetEventLocationHandler( IVeaEventRepository eventRepository, ILocationRepository locationRepository)
    {
        _addLocationToEvent = new AddLocationToEvent(locationRepository, eventRepository);
    }
    
    public async Task<Result> HandleAsync(SetEventLocationCommand command)
    {
        return await _addLocationToEvent.Handle(command.LocationId, command.EventId);
    }
}