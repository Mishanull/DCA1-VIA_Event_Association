using ViaEventAssociation.Core.Domain.Common.Base;
using ViaEventAssociation.Core.Domain.EventAgg;
using ViaEventsAssociation.Core.Application.CommandDispatching.Common.Base;
using VIAEventsAssociation.Core.Tools.OperationResult.OperationResult;

namespace ViaEventsAssociation.Core.Application.CommandDispatching.Commands.Event;

public class UpdateDescriptionCommand : Command
{
    internal VeaEventId EventId { get; }
    internal Description Description { get; }
    
    private UpdateDescriptionCommand(VeaEventId eventId, Description description)
    {
        EventId = eventId;
        Description = description;
    }
    
    public static Result<UpdateDescriptionCommand> Create(string eventId, string description)
    {
        var veaEventIdResult = TId.FromString<VeaEventId>(eventId);
        var veaEventDescriptionResult = Description.Create(description);
        
        var result = new Result<UpdateDescriptionCommand>(null);
        result.CollectFromMultiple(veaEventIdResult, veaEventDescriptionResult);
        
        return result.IsErrorResult() ? result : new Result<UpdateDescriptionCommand>(new UpdateDescriptionCommand(veaEventIdResult.Value!, veaEventDescriptionResult.Value!));
    }
    
    protected override IEnumerable<object?> GetEqualityComponents()
    {
        yield return EventId;
        yield return Description;
    }
}