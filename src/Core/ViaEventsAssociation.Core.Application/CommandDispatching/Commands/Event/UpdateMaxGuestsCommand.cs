using ViaEventAssociation.Core.Domain.Common.Base;
using ViaEventAssociation.Core.Domain.EventAgg;
using ViaEventsAssociation.Core.Application.CommandDispatching.Common.Base;
using VIAEventsAssociation.Core.Tools.OperationResult.OperationResult;

namespace ViaEventsAssociation.Core.Application.CommandDispatching.Commands.Event;

public class UpdateMaxGuestsCommand : Command
{
    internal VeaEventId EventId { get; }
    internal MaxGuests MaxGuests { get; }
    
    private UpdateMaxGuestsCommand(VeaEventId eventId, MaxGuests maxGuests)
    {
        EventId = eventId;
        MaxGuests = maxGuests;
    }
    
    public static Result<UpdateMaxGuestsCommand> Create(string eventId, int maxGuests)
    {
        VeaEventId veaEventId = TId.FromString(eventId) as VeaEventId;
        Result<MaxGuests> veaEventMaxGuests = MaxGuests.Create(maxGuests);

        var result = new Result<UpdateMaxGuestsCommand>(null);
        result.CollectErrors(veaEventMaxGuests.Errors);
        
        return result.IsErrorResult() ? result : new Result<UpdateMaxGuestsCommand>(new UpdateMaxGuestsCommand(veaEventId, veaEventMaxGuests.Value));
    }
    
    protected override IEnumerable<object?> GetEqualityComponents()
    {
        throw new NotImplementedException();
    }
}