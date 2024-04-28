using ViaEventAssociation.Core.Domain.CreatorAgg;
using ViaEventAssociation.Core.Domain.EventAgg;
using ViaEventAssociation.Core.Domain.GuestAgg;
using ViaEventAssociation.Core.Domain.Services.Guest;
using ViaEventsAssociation.Core.Application.CommandHandler.Commands.Guest;
using ViaEventsAssociation.Core.Application.CommandHandler.Common.Base;
using VIAEventsAssociation.Core.Tools.OperationResult.OperationResult;

namespace ViaEventsAssociation.Core.Application.CommandHandler.Handlers.GuestHandlers;

internal class GuestDeclinesInviteHandler : ICommandHandler<GuestDeclinesInviteCommand>
{
    private readonly GuestDeclinesInvite _guestDeclinesInvite;

    public GuestDeclinesInviteHandler(IGuestRepository guestRepository, IVeaEventRepository eventRepository, ICreatorRepository creatorRepository)
    {
        _guestDeclinesInvite = new GuestDeclinesInvite(guestRepository, creatorRepository, eventRepository);
    }

    public async Task<Result> HandleAsync(GuestDeclinesInviteCommand command)
    {
        var result = await _guestDeclinesInvite.Handle(command.InviteId);
        if (result.IsErrorResult())
        {
            return result;
        }
        
        return result;
    }
}