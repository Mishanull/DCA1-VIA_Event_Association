using ViaEventAssociation.Core.Domain.Common.Base;
using ViaEventAssociation.Core.Domain.EventAgg;
using ViaEventsAssociation.Core.Application.CommandDispatching.Common.Base;

namespace ViaEventsAssociation.Core.Application.CommandDispatching.Commands.Event;

public class ActivateCommand : Command
{
    internal VeaEventId EventId { get; }
    
    private ActivateCommand(VeaEventId eventId)
    {
        EventId = eventId;
    }
    
    public static ActivateCommand Create(string eventId)
    {
        VeaEventId veaEventId = TId.FromString(eventId) as VeaEventId;
        return new ActivateCommand(veaEventId);
    }
    
    protected override IEnumerable<object?> GetEqualityComponents()
    {
        throw new NotImplementedException();
    }
}