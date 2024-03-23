using ViaEventAssociation.Core.Domain.Contracts.Repositories;
using ViaEventAssociation.Core.Domain.Contracts.UnitOfWork;
using ViaEventAssociation.Core.Domain.Services;
using ViaEventsAssociation.Core.Application.CommandDispatching.Commands.Guest;
using ViaEventsAssociation.Core.Application.CommandDispatching.Common.Base;
using VIAEventsAssociation.Core.Tools.OperationResult.OperationResult;

namespace ViaEventsAssociation.Core.Application.CommandDispatching.Handlers.Guest;

public class GuestCancelsEventParticipationHandler : ICommandHandler<GuestCancelsEventParticipationCommand>
{
    private readonly IUnitOfWork _uow;
    private readonly GuestCancelsEventParticipation _guestCancelsEventParticipation;

    internal GuestCancelsEventParticipationHandler(IUnitOfWork uow, IGuestRepository guestRepository, IVeaEventRepository eventRepository, IRequestRepository requestRepository)
    {
        _uow = uow;
        _guestCancelsEventParticipation = new GuestCancelsEventParticipation(guestRepository, eventRepository, requestRepository);
    }
    
    public async Task<Result> HandleAsync(GuestCancelsEventParticipationCommand command)
    {
        var result = await _guestCancelsEventParticipation.Handle(command.RequestId);
        if (result.IsErrorResult())
        {
            return result;
        }
        
        await _uow.SaveChangesAsync();
        return result;
    }
}