using ViaEventAssociation.Core.Domain.Common.Base;
using ViaEventAssociation.Core.Domain.Common.ValueObjects;
using ViaEventAssociation.Core.Domain.Contracts;
using ViaEventAssociation.Core.Domain.CreatorAgg;
using ViaEventsAssociation.Core.Application.CommandDispatching.Common.Base;
using VIAEventsAssociation.Core.Tools.OperationResult.OperationResult;

namespace ViaEventsAssociation.Core.Application.CommandDispatching.Commands.Event;

public class CreateEventCommand : Command
{
    internal ICurrentTime CurrentTime { get; }
    internal CreatorId CreatorId { get; }

    private CreateEventCommand(ICurrentTime currentTime, CreatorId creatorId)
    {
        CurrentTime = currentTime;
        CreatorId = creatorId;
    }

    public static Result<CreateEventCommand> Create(string creatorId, ICurrentTime currentTime)
    {
        var creatorIdResult = TId.FromString<CreatorId>(creatorId);
        
        var result = new Result<CreateEventCommand>(null);
        result.CollectErrors(creatorIdResult.Errors);
        
        return result.IsErrorResult() ? result : new Result<CreateEventCommand>(new CreateEventCommand(currentTime, creatorIdResult.Value!));
    }
    
    protected override IEnumerable<object?> GetEqualityComponents()
    {
        yield return CurrentTime;
    }
}