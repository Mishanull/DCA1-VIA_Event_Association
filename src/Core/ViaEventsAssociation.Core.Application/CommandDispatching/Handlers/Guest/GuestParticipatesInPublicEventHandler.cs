using ViaEventAssociation.Core.Domain.Contracts.Repositories;
using ViaEventAssociation.Core.Domain.Contracts.UnitOfWork;
using ViaEventAssociation.Core.Domain.GuestAgg.RequestEntity;
using ViaEventAssociation.Core.Domain.Services;
using ViaEventsAssociation.Core.Application.CommandDispatching.Commands.Guest;
using ViaEventsAssociation.Core.Application.CommandDispatching.Common.Base;
using VIAEventsAssociation.Core.Tools.OperationResult.OperationResult;

namespace ViaEventsAssociation.Core.Application.CommandDispatching.Handlers.Guest;

internal class GuestParticipatesInPublicEventHandler : ICommandHandler<GuestParticipatesInPublicEventCommand>
{
    private readonly IUnitOfWork _uow;
    private readonly GuestRequestParticipationPublicEvent _guestRequestParticipation;
    private readonly IRequestRepository _requestRepository;

    internal GuestParticipatesInPublicEventHandler(IUnitOfWork uow, IGuestRepository guestRepository, IVeaEventRepository eventRepository, IRequestRepository requestRepository)
    {
        _uow = uow;
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

        await _uow.SaveChangesAsync();
        return handleResult;
    }
}
