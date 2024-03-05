using UnitTests.FakeServices;
using ViaEventAssociation.Core.Domain.EventAgg;
using ViaEventAssociation.Core.Domain.Common.ValueObjects;
using VIAEventsAssociation.Core.Tools.OperationResult.OperationResult;

namespace UnitTests;

public class VeaEventBuilder
{
    public static VeaEvent? VeaEvent;
    
    public VeaEventBuilder Init()
    {
        // VeaEvent = new VeaEvent(new VeaEventId());
        VeaEvent = VeaEvent.Create(new FakeCurrentTime()).Value;
        return this;
    }
    
    public VeaEventBuilder WithTime(DateTime fakeTime)
    {
        ((FakeCurrentTime)VeaEvent.currentTimeProvider).FakeTime = fakeTime;
        return this;
    }
    
    public VeaEventBuilder WithTitle(string title)
    {
        VeaEvent.Title = Title.Create(title).Value;
        return this;
    }
    
    public VeaEventBuilder WithDescription(string description)
    {
        VeaEvent.Description = Description.Create(description).Value;
        return this;
    }
    
    public VeaEventBuilder WithEventType(VeaEventType veaEventType)
    {
        VeaEvent.VeaEventType = veaEventType;
        return this;
    }
    
    public VeaEventBuilder WithMaxGuests(int maxGuests)
    {
        VeaEvent.MaxGuests = ((Result<MaxGuests>)MaxGuests.Create(maxGuests)).Value;
        return this;
    }
    
    public VeaEventBuilder WithStatus(VeaEventStatus veaEventStatus)
    {
        VeaEvent.VeaEventStatus = veaEventStatus;
        return this;
    }
    
    public VeaEventBuilder WithFromTo(DateTime from, DateTime to)
    {
        VeaEvent.FromTo = ((Result<FromTo>)FromTo.Create(from, to)).Value;
        return this;
    }
    
    public VeaEvent Build()
    {
        return VeaEvent;
    }
    
}