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
        VeaEventId veaEventId = TId.FromString(eventId) as VeaEventId;
        Result<FromTo> veaEventFromTo = FromTo.Create(from, to);

        var result = new Result<UpdateFromToCommand>(null);
        result.CollectErrors(veaEventFromTo.Errors);
        
        return result.IsErrorResult() ? result : new Result<UpdateFromToCommand>(new UpdateFromToCommand(veaEventId, veaEventFromTo.Value));
    }
    
    protected override IEnumerable<object?> GetEqualityComponents()
    {
        throw new NotImplementedException();
    }
}