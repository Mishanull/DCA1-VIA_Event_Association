using ViaEventAssociation.Core.Domain.Common.Base;
using ViaEventAssociation.Core.Domain.EventAgg;
using ViaEventsAssociation.Core.Application.CommandHandler.Common.Base;
using VIAEventsAssociation.Core.Tools.OperationResult.OperationResult;

namespace ViaEventsAssociation.Core.Application.CommandHandler.Commands.Event;

public class ActivateEventCommand : Command
{
    internal VeaEventId VeaEventId { get; }
    
    private ActivateEventCommand(VeaEventId veaEventId)
    {
        VeaEventId = veaEventId;
    }
    
    public static Result<ActivateEventCommand> Create(string eventId)
    {
        var veaEventIdResult = TId.FromString<VeaEventId>(eventId);
        
        var result = new Result<ActivateEventCommand>(null);
        result.CollectErrors(veaEventIdResult.Errors);
        
        return result.IsErrorResult() ? result : new Result<ActivateEventCommand>(new ActivateEventCommand(veaEventIdResult.Value!));
    }
    
    protected override IEnumerable<object?> GetEqualityComponents()
    {
        yield return VeaEventId;
    }
}