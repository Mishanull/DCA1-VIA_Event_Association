using ViaEventAssociation.Core.Domain.Common.Base;
using ViaEventAssociation.Core.Domain.CreatorAgg.InviteEntity;
using ViaEventsAssociation.Core.Application.CommandDispatching.Common.Base;
using VIAEventsAssociation.Core.Tools.OperationResult.OperationResult;

namespace ViaEventsAssociation.Core.Application.CommandDispatching.Commands.Guest;

public class GuestAcceptsInviteCommand : Command
{
    internal InviteId InviteId { get; private init; }

    public static Result<GuestAcceptsInviteCommand> Create(string inviteId)
    {
        var inviteIdCreationResult = TId.FromString<InviteId>(inviteId);
        if (inviteIdCreationResult.IsErrorResult())
        {
            var result = new Result<GuestAcceptsInviteCommand>(null);
            result.CollectErrors(inviteIdCreationResult.Errors);
            return result;
        }

        return new Result<GuestAcceptsInviteCommand>(new GuestAcceptsInviteCommand()
        {
            InviteId =  inviteIdCreationResult.Value!
        });
    }

    protected override IEnumerable<object?> GetEqualityComponents()
    {
        yield return InviteId;
    }
}