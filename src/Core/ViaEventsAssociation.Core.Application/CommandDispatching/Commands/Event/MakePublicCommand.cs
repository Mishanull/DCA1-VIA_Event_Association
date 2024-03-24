using ViaEventAssociation.Core.Domain.Common.Base;
using ViaEventAssociation.Core.Domain.EventAgg;
using ViaEventsAssociation.Core.Application.CommandDispatching.Common.Base;

namespace ViaEventsAssociation.Core.Application.CommandDispatching.Commands.Event;

public class MakePublicCommand : Command
{
    internal VeaEventId EventId { get; }
    
    private MakePublicCommand(VeaEventId eventId)
    {
        EventId = eventId;
    }
    
    public static MakePublicCommand Create(string eventId)
    {
        VeaEventId veaEventId = TId.FromString(eventId) as VeaEventId;
        return new MakePublicCommand(veaEventId);
    }
    
    protected override IEnumerable<object?> GetEqualityComponents()
    {
        throw new NotImplementedException();
    }
}