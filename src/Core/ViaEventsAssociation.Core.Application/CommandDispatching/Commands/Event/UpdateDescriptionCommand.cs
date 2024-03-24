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
        VeaEventId veaEventId = TId.FromString(eventId) as VeaEventId;
        Result<Description> veaEventDescription = Description.Create(description);

        var result = new Result<UpdateDescriptionCommand>(null);
        result.CollectErrors(veaEventDescription.Errors);
        
        return result.IsErrorResult() ? result : new Result<UpdateDescriptionCommand>(new UpdateDescriptionCommand(veaEventId, veaEventDescription.Value));
    }
    
    protected override IEnumerable<object?> GetEqualityComponents()
    {
        throw new NotImplementedException();
    }
}