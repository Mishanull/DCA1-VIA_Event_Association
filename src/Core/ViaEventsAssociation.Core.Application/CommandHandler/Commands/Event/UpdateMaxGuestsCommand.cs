﻿using ViaEventAssociation.Core.Domain.Common.Base;
using ViaEventAssociation.Core.Domain.Common.ValueObjects;
using ViaEventAssociation.Core.Domain.EventAgg;
using ViaEventsAssociation.Core.Application.CommandHandler.Common.Base;
using VIAEventsAssociation.Core.Tools.OperationResult.OperationResult;

namespace ViaEventsAssociation.Core.Application.CommandHandler.Commands.Event;

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
        var veaEventIdResult = TId.FromString<VeaEventId>(eventId);
        var veaEventMaxGuestsResult = MaxGuests.Create(maxGuests);

        var result = new Result<UpdateMaxGuestsCommand>(null);
        result.CollectFromMultiple(veaEventIdResult, veaEventMaxGuestsResult);
        
        return result.IsErrorResult() ? result : new Result<UpdateMaxGuestsCommand>(new UpdateMaxGuestsCommand(veaEventIdResult.Value!, veaEventMaxGuestsResult.Value!));
    }
    
    protected override IEnumerable<object?> GetEqualityComponents()
    {
        yield return EventId;
        yield return MaxGuests;
    }
}