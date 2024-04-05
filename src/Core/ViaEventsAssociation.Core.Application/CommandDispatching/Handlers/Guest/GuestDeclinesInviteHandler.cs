using ViaEventAssociation.Core.Domain.Contracts.Repositories;
using ViaEventAssociation.Core.Domain.Contracts.UnitOfWork;
using ViaEventAssociation.Core.Domain.Services;
using ViaEventsAssociation.Core.Application.CommandDispatching.Commands.Guest;
using ViaEventsAssociation.Core.Application.CommandDispatching.Common.Base;
using VIAEventsAssociation.Core.Tools.OperationResult.OperationResult;

namespace ViaEventsAssociation.Core.Application.CommandDispatching.Handlers.Guest;

internal class GuestDeclinesInviteHandler : ICommandHandler<GuestDeclinesInviteCommand>
{
    private readonly IUnitOfWork _uow;
    private readonly GuestDeclinesInvite _guestDeclinesInvite;

    internal GuestDeclinesInviteHandler(IGuestRepository guestRepository, IVeaEventRepository eventRepository, IInviteRepository inviteRepository, ICreatorRepository creatorRepository, IUnitOfWork uow)
    {
        _uow = uow;
        _guestDeclinesInvite = new GuestDeclinesInvite(guestRepository, creatorRepository, eventRepository, inviteRepository);
    }

    public async Task<Result> HandleAsync(GuestDeclinesInviteCommand command)
    {
        var result = await _guestDeclinesInvite.Handle(command.InviteId);
        if (result.IsErrorResult())
        {
            return result;
        }
        
        await _uow.SaveChangesAsync();
        return result;
    }
}