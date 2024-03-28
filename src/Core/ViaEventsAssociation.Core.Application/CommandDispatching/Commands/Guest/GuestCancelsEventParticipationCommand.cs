using ViaEventAssociation.Core.Domain.Common.Base;
using ViaEventAssociation.Core.Domain.GuestAgg.Request;
using ViaEventsAssociation.Core.Application.CommandDispatching.Common.Base;
using VIAEventsAssociation.Core.Tools.OperationResult.OperationResult;

namespace ViaEventsAssociation.Core.Application.CommandDispatching.Commands.Guest;

public class GuestCancelsEventParticipationCommand : Command
{
    internal RequestId RequestId { get; private init; }

    public static Result<GuestCancelsEventParticipationCommand> Create(string requestId)
    {
        var requestIdCreationResult = TId.FromString<RequestId>(requestId);
        if (requestIdCreationResult.IsErrorResult())
        {
            var result = new Result<GuestCancelsEventParticipationCommand>(null);
            result.CollectErrors(requestIdCreationResult.Errors);
            return result;
        }

        return new Result<GuestCancelsEventParticipationCommand>(new GuestCancelsEventParticipationCommand()
        {
            RequestId = (RequestId)requestIdCreationResult.Value!
        });
    }

    protected override IEnumerable<object?> GetEqualityComponents()
    {
        yield return RequestId;

    }
}