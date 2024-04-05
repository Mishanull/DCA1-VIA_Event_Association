using ViaEventAssociation.Core.Domain.Common.Base;
using ViaEventAssociation.Core.Domain.CreatorAgg.InviteEntity;
using ViaEventsAssociation.Core.Application.CommandHandler.Common.Base;
using VIAEventsAssociation.Core.Tools.OperationResult.OperationResult;

namespace ViaEventsAssociation.Core.Application.CommandHandler.Commands.Guest;

public class GuestDeclinesInviteCommand : Command
{
    internal InviteId InviteId { get; private init; }

    public static Result<GuestDeclinesInviteCommand> Create(string inviteId)
    {
        var inviteIdCreationResult = TId.FromString<InviteId>(inviteId);
        if (inviteIdCreationResult.IsErrorResult())
        {
            var result = new Result<GuestDeclinesInviteCommand>(null);
            result.CollectErrors(inviteIdCreationResult.Errors);
            return result;
        }

        return new Result<GuestDeclinesInviteCommand>(new GuestDeclinesInviteCommand()
        {
            InviteId = (InviteId) inviteIdCreationResult.Value!
        });
    }

    protected override IEnumerable<object?> GetEqualityComponents()
    {
        yield return InviteId;
    }

}