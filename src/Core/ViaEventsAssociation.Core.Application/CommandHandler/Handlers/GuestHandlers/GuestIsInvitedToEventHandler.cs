using ViaEventAssociation.Core.Domain.Contracts.Repositories;
using ViaEventAssociation.Core.Domain.CreatorAgg.InviteEntity;
using ViaEventAssociation.Core.Domain.Services.Guest;
using ViaEventsAssociation.Core.Application.CommandHandler.Commands.Guest;
using ViaEventsAssociation.Core.Application.CommandHandler.Common.Base;
using VIAEventsAssociation.Core.Tools.OperationResult.OperationResult;

namespace ViaEventsAssociation.Core.Application.CommandHandler.Handlers.GuestHandlers;

internal class GuestIsInvitedToEventHandler : ICommandHandler<GuestIsInvitedToEventCommand>
{
    private readonly GuestIsInvitedToEvent _guestIsInvitedToEvent;

    internal GuestIsInvitedToEventHandler(IGuestRepository guestRepo, ICreatorRepository creatorRepo, IVeaEventRepository eventRepo, IInviteRepository inviteRepo)
    {
        _guestIsInvitedToEvent = new GuestIsInvitedToEvent(guestRepo, creatorRepo, eventRepo, inviteRepo);
    }

    public async Task<Result> HandleAsync(GuestIsInvitedToEventCommand command)
    {
        var createResult = Invite.Create(command.GuestId, command.CreatorId, command.EventId);
        if (createResult.IsErrorResult())
        {
            return createResult;
        }

        var handleResult = await _guestIsInvitedToEvent.Handle(createResult.Value!);
        if (handleResult.IsErrorResult())
        {
            return handleResult;
        }

        return new();
    }
}