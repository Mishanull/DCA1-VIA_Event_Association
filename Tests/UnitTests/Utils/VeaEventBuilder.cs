using UnitTests.FakeServices;
using ViaEventAssociation.Core.Domain.Common.Base;
using ViaEventAssociation.Core.Domain.Common.ValueObjects;
using ViaEventAssociation.Core.Domain.CreatorAgg;
using ViaEventAssociation.Core.Domain.EventAgg;

namespace UnitTests.Utils;

public class VeaEventBuilder
{
    private static VeaEvent? _veaEvent;
    
    public VeaEventBuilder Init()
    {
        _veaEvent = VeaEvent.Create(new CreatorId() ,new FakeCurrentTime()).Value;
        return this;
    }
    
    public VeaEventBuilder WithTime(DateTime fakeTime)
    {
        ((FakeCurrentTime)_veaEvent.CurrentTimeProvider).FakeTime = fakeTime;
        return this;
    }
    
    public VeaEventBuilder WithTitle(string title)
    {
        _veaEvent.Title = Title.Create(title).Value;
        return this;
    }
    
    public VeaEventBuilder WithDescription(string description)
    {
        _veaEvent.Description = Description.Create(description).Value;
        return this;
    }
    
    public VeaEventBuilder WithEventType(VeaEventType veaEventType)
    {
        _veaEvent.VeaEventType = veaEventType;
        return this;
    }
    
    public VeaEventBuilder WithMaxGuests(int maxGuests)
    {
        _veaEvent.MaxGuests = MaxGuests.Create(maxGuests).Value;
        return this;
    }
    
    public VeaEventBuilder WithStatus(VeaEventStatus veaEventStatus)
    {
        _veaEvent.VeaEventStatus = veaEventStatus;
        return this;
    }
    
    public VeaEventBuilder WithFromTo(DateTime from, DateTime to)
    {
        _veaEvent.FromTo = FromTo.Create(from, to).Value;
        return this;
    }

    public VeaEventBuilder WithCreatorId(string creatorId)
    {
        var creatorIdResult = TId.FromString<CreatorId>(creatorId);
        _veaEvent.CreatorId = creatorIdResult.Value!;
        return this;
    }
    
    public VeaEvent Build()
    {
        return _veaEvent;
    }
    
}