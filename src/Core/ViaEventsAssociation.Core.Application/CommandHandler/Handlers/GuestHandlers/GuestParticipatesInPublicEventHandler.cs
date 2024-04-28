using ViaEventAssociation.Core.Domain.EventAgg;
using ViaEventAssociation.Core.Domain.GuestAgg;
using ViaEventAssociation.Core.Domain.GuestAgg.RequestEntity;
using ViaEventAssociation.Core.Domain.Services.Guest;
using ViaEventsAssociation.Core.Application.CommandHandler.Commands.Guest;
using ViaEventsAssociation.Core.Application.CommandHandler.Common.Base;
using VIAEventsAssociation.Core.Tools.OperationResult.OperationResult;

namespace ViaEventsAssociation.Core.Application.CommandHandler.Handlers.GuestHandlers;

internal class GuestParticipatesInPublicEventHandler : ICommandHandler<GuestParticipatesInPublicEventCommand>
{
    private readonly GuestRequestParticipationPublicEvent _guestRequestParticipation;

    public GuestParticipatesInPublicEventHandler(IGuestRepository guestRepository, IVeaEventRepository eventRepository)
    {
        _guestRequestParticipation = new GuestRequestParticipationPublicEvent(guestRepository, eventRepository);
    }

    public async Task<Result> HandleAsync(GuestParticipatesInPublicEventCommand command)
    {
        var createRequestResult = Request.Create(command.Reason, command.EventId, command.VeaGuestId);
        if (createRequestResult.IsErrorResult())
        {
            return createRequestResult;
        }

        var handleResult = await _guestRequestParticipation.Handle(createRequestResult.Value!);
        if (handleResult.IsErrorResult())
        {
            return handleResult;
        }

        return handleResult;
    }
}
