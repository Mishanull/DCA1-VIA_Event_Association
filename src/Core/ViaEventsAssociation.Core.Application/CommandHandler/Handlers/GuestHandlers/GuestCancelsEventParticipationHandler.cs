using ViaEventAssociation.Core.Domain.EventAgg;
using ViaEventAssociation.Core.Domain.GuestAgg;
using ViaEventAssociation.Core.Domain.Services.Guest;
using ViaEventsAssociation.Core.Application.CommandHandler.Commands.Guest;
using ViaEventsAssociation.Core.Application.CommandHandler.Common.Base;
using VIAEventsAssociation.Core.Tools.OperationResult.OperationResult;

namespace ViaEventsAssociation.Core.Application.CommandHandler.Handlers.GuestHandlers;

public class GuestCancelsEventParticipationHandler : ICommandHandler<GuestCancelsEventParticipationCommand>
{
    private readonly GuestCancelsEventParticipation _guestCancelsEventParticipation;

    internal GuestCancelsEventParticipationHandler(IGuestRepository guestRepository, IVeaEventRepository eventRepository)
    {
        _guestCancelsEventParticipation = new GuestCancelsEventParticipation(guestRepository, eventRepository);
    }
    
    public async Task<Result> HandleAsync(GuestCancelsEventParticipationCommand command)
    {
        var result = await _guestCancelsEventParticipation.Handle(command.RequestId);
        if (result.IsErrorResult())
        {
            return result;
        }
        
        return result;
    }
}