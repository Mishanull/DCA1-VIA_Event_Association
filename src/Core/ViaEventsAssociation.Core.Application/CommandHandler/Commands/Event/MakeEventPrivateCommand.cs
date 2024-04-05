using ViaEventAssociation.Core.Domain.Common.Base;
using ViaEventAssociation.Core.Domain.EventAgg;
using ViaEventsAssociation.Core.Application.CommandHandler.Common.Base;
using VIAEventsAssociation.Core.Tools.OperationResult.OperationResult;

namespace ViaEventsAssociation.Core.Application.CommandHandler.Commands.Event;

public class MakeEventPrivateCommand : Command
{
    internal VeaEventId EventId { get; }
    
    private MakeEventPrivateCommand(VeaEventId eventId)
    {
        EventId = eventId;
    }
    
    public static Result<MakeEventPrivateCommand> Create(string eventId)
    {
        var veaEventIdResult = TId.FromString<VeaEventId>(eventId);
        
        var result = new Result<MakeEventPrivateCommand>(null);
        result.CollectErrors(veaEventIdResult.Errors);
        
        return result.IsErrorResult() ? result : new Result<MakeEventPrivateCommand>(new MakeEventPrivateCommand(veaEventIdResult.Value!));
    }
    
    protected override IEnumerable<object?> GetEqualityComponents()
    {
        yield return EventId;
    }
}