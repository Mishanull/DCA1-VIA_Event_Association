using ViaEventAssociation.Core.Domain.Contracts.Repositories;
using ViaEventAssociation.Core.Domain.Services.Guest;
using ViaEventsAssociation.Core.Application.CommandHandler.Commands.Guest;
using ViaEventsAssociation.Core.Application.CommandHandler.Common.Base;
using VIAEventsAssociation.Core.Tools.OperationResult.OperationResult;

namespace ViaEventsAssociation.Core.Application.CommandHandler.Handlers.GuestHandlers;

internal class GuestAcceptsInviteHandler : ICommandHandler<GuestAcceptsInviteCommand>
{
    private readonly GuestAcceptsInvite _guestAcceptsInvite;

    internal GuestAcceptsInviteHandler(IGuestRepository guestRepository, IVeaEventRepository eventRepository, ICreatorRepository creatorRepository)
    {
        _guestAcceptsInvite = new GuestAcceptsInvite(guestRepository, creatorRepository, eventRepository);
    }

    public async Task<Result> HandleAsync(GuestAcceptsInviteCommand command)
    {
        var result = await _guestAcceptsInvite.Handle(command.InviteId);
        if (result.IsErrorResult())
        {
            return result;
        }
        
        return result;
    }
}