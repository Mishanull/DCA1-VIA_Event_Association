using ViaEventAssociation.Core.Domain.Contracts.Repositories;
using ViaEventAssociation.Core.Domain.GuestAgg.RequestEntity;
using ViaEventAssociation.Core.Domain.Services.Guest;
using ViaEventsAssociation.Core.Application.CommandHandler.Commands.Guest;
using ViaEventsAssociation.Core.Application.CommandHandler.Common.Base;
using VIAEventsAssociation.Core.Tools.OperationResult.OperationResult;

namespace ViaEventsAssociation.Core.Application.CommandHandler.Handlers.GuestHandlers;

internal class GuestParticipatesInPublicEventHandler : ICommandHandler<GuestParticipatesInPublicEventCommand>
{
    private readonly GuestRequestParticipationPublicEvent _guestRequestParticipation;
    private readonly IRequestRepository _requestRepository;

    internal GuestParticipatesInPublicEventHandler(IGuestRepository guestRepository, IVeaEventRepository eventRepository, IRequestRepository requestRepository)
    {
        _requestRepository = requestRepository;
        _guestRequestParticipation = new GuestRequestParticipationPublicEvent(guestRepository, eventRepository, requestRepository);
    }

    public async Task<Result> HandleAsync(GuestParticipatesInPublicEventCommand command)
    {
        var createRequestResult = Request.Create(command.Reason, command.EventId, command.VeaGuestId);
        if (createRequestResult.IsErrorResult())
        {
            return createRequestResult;
        }

        var repoAddResult = await _requestRepository.AddAsync(createRequestResult.Value!);
        if (repoAddResult.IsErrorResult())
        {
            return repoAddResult;
        }

        var handleResult = await _guestRequestParticipation.Handle(createRequestResult.Value!);
        if (handleResult.IsErrorResult())
        {
            return handleResult;
        }

        return handleResult;
    }
}
