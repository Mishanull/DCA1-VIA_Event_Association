using ViaEventAssociation.Core.Domain.Common.Base;
using ViaEventAssociation.Core.Domain.Common.ValueObjects;
using ViaEventAssociation.Core.Domain.EventAgg;
using ViaEventsAssociation.Core.Application.CommandDispatching.Common.Base;
using VIAEventsAssociation.Core.Tools.OperationResult.OperationResult;

namespace ViaEventsAssociation.Core.Application.CommandDispatching.Commands.Event;

public class UpdateFromToCommand : Command
{
    internal VeaEventId EventId { get; }
    internal FromTo FromTo { get; }
    
    private UpdateFromToCommand(VeaEventId eventId, FromTo fromTo)
    {
        EventId = eventId;
        FromTo = fromTo;
    }
    
    public static Result<UpdateFromToCommand> Create(string eventId, DateTime from, DateTime to)
    {
        var veaEventIdResult = TId.FromString<VeaEventId>(eventId);
        var veaEventFromToResult = FromTo.Create(from, to);
        
        var result = new Result<UpdateFromToCommand>(null);
        result.CollectFromMultiple(veaEventIdResult, veaEventFromToResult);
        
        return result.IsErrorResult() ? result : new Result<UpdateFromToCommand>(new UpdateFromToCommand(veaEventIdResult.Value!, veaEventFromToResult.Value!));
    }
    
    protected override IEnumerable<object?> GetEqualityComponents()
    {
        yield return EventId;
        yield return FromTo;
    }
}