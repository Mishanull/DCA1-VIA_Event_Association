using ViaEventAssociation.Core.Domain.Common.Base;
using ViaEventAssociation.Core.Domain.EventAgg;
using ViaEventsAssociation.Core.Application.CommandDispatching.Common.Base;
using VIAEventsAssociation.Core.Tools.OperationResult.OperationResult;

namespace ViaEventsAssociation.Core.Application.CommandDispatching.Commands.Event;

public class UpdateTitleCommand : Command
{
    internal VeaEventId EventId { get; }
    internal Title Title { get; }

    private UpdateTitleCommand(VeaEventId eventId, Title title)
    {
        EventId = eventId;
        Title = title;
    }
    
    public static Result<UpdateTitleCommand> Create(string eventId, string title)
    {
        VeaEventId veaEventId = TId.FromString(eventId) as VeaEventId;
        Result<Title> veaEventTitle = Title.Create(title);

        var result = new Result<UpdateTitleCommand>(null);
        result.CollectErrors(veaEventTitle.Errors);
        
        return result.IsErrorResult() ? result : new Result<UpdateTitleCommand>(new UpdateTitleCommand(veaEventId, veaEventTitle.Value));
    }
    
    protected override IEnumerable<object?> GetEqualityComponents()
    {
        throw new NotImplementedException();
    }
}