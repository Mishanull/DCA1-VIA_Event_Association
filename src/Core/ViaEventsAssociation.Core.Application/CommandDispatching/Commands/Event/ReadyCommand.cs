using ViaEventAssociation.Core.Domain.Common.Base;
using ViaEventAssociation.Core.Domain.EventAgg;
using ViaEventsAssociation.Core.Application.CommandDispatching.Common.Base;

namespace ViaEventsAssociation.Core.Application.CommandDispatching.Commands.Event;

public class ReadyCommand : Command
{
    internal VeaEventId EventId { get; }
    
    private ReadyCommand(VeaEventId eventId)
    {
        EventId = eventId;
    }
    
    public static ReadyCommand Create(string eventId)
    {
        VeaEventId veaEventId = TId.FromString(eventId) as VeaEventId;
        return new ReadyCommand(veaEventId);
    }
    
    protected override IEnumerable<object?> GetEqualityComponents()
    {
        throw new NotImplementedException();
    }
}