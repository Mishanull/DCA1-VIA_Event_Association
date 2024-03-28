using ViaEventAssociation.Core.Domain.Contracts.Repositories;
using ViaEventAssociation.Core.Domain.Contracts.UnitOfWork;
using ViaEventAssociation.Core.Domain.Services;
using ViaEventsAssociation.Core.Application.CommandDispatching.Commands.Guest;
using ViaEventsAssociation.Core.Application.CommandDispatching.Common.Base;
using VIAEventsAssociation.Core.Tools.OperationResult.OperationResult;

namespace ViaEventsAssociation.Core.Application.CommandDispatching.Handlers.Guest;

internal class GuestAcceptsInviteHandler : ICommandHandler<GuestAcceptsInviteCommand>
{
    private readonly IUnitOfWork _uow;
    private readonly GuestAcceptsInvite _guestAcceptsInvite;

    internal GuestAcceptsInviteHandler(IGuestRepository guestRepository, IVeaEventRepository eventRepository, IInviteRepository inviteRepository, ICreatorRepository creatorRepository, IUnitOfWork uow)
    {
        _uow = uow;
        _guestAcceptsInvite = new GuestAcceptsInvite(guestRepository, creatorRepository, eventRepository, inviteRepository);
    }

    public async Task<Result> HandleAsync(GuestAcceptsInviteCommand command)
    {
        var result = await _guestAcceptsInvite.Handle(command.InviteId);
        if (result.IsErrorResult())
        {
            return result;
        }
        
        await _uow.SaveChangesAsync();
        return result;
    }
}