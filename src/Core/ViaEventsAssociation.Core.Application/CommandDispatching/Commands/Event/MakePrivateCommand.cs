using ViaEventAssociation.Core.Domain.Common.Base;
using ViaEventAssociation.Core.Domain.EventAgg;
using ViaEventsAssociation.Core.Application.CommandDispatching.Common.Base;

namespace ViaEventsAssociation.Core.Application.CommandDispatching.Commands.Event;

public class MakePrivateCommand : Command
{
    internal VeaEventId EventId { get; }
    
    private MakePrivateCommand(VeaEventId eventId)
    {
        EventId = eventId;
    }
    
    public static MakePrivateCommand Create(string eventId)
    {
        VeaEventId veaEventId = TId.FromString(eventId) as VeaEventId;

        return new MakePrivateCommand(veaEventId);
    }
    
    protected override IEnumerable<object?> GetEqualityComponents()
    {
        throw new NotImplementedException();
    }
}