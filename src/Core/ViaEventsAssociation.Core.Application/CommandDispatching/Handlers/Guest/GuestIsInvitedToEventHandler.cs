using ViaEventAssociation.Core.Domain.Contracts.Repositories;
using ViaEventAssociation.Core.Domain.Contracts.UnitOfWork;
using ViaEventAssociation.Core.Domain.CreatorAgg.InviteEntity;
using ViaEventAssociation.Core.Domain.Services;
using ViaEventsAssociation.Core.Application.CommandDispatching.Commands.Guest;
using ViaEventsAssociation.Core.Application.CommandDispatching.Common.Base;
using VIAEventsAssociation.Core.Tools.OperationResult.OperationResult;

namespace ViaEventsAssociation.Core.Application.CommandDispatching.Handlers.Guest;

internal class GuestIsInvitedToEventHandler : ICommandHandler<GuestIsInvitedToEventCommand>
{
    private readonly IUnitOfWork _uow;
    private readonly GuestIsInvitedToEvent _guestIsInvitedToEvent;

    public GuestIsInvitedToEventHandler(IGuestRepository guestRepo, ICreatorRepository creatorRepo, IVeaEventRepository eventRepo, IInviteRepository inviteRepo, IUnitOfWork uow)
    {
        _uow = uow;
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

        await _uow.SaveChangesAsync();
        return new();
    }
}