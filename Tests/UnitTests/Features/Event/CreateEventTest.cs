using ViaEventAssociation.Core.Domain.EventAgg;

namespace UnitTests.Features.Event;

public class CreateEventTest
{

    [Fact]
    public void S1_CreateEvent_WithNoParameters_ShouldCreateDraftEventWithDefaultMaxNumberOfGuests()
    {
        // Arrange
        var veaEvent = new VeaEvent(new VeaEventId());
        var defaultMaxGuests = 5;
        
        // Assert
        Assert.NotNull(veaEvent.Id);
        Assert.Equal(veaEvent.VeaEventStatus, VeaEventStatus.Draft);
        Assert.Equal(veaEvent.MaxGuests.Value, defaultMaxGuests);
    }
    
    [Fact]
    public void S2_CreateEvent_WithoutTitle_ShouldCreateEventWithDefaultTitle()
    {
        // Arrange
        var veaEvent = new VeaEvent(new VeaEventId());
        var defaultTitle = "Working Title";
        
        // Assert
        Assert.Equal(veaEvent.Title.Value, defaultTitle);
    }
    
    [Fact]
    public void S3_CreateEvent_WithoutDescription_ShouldCreateEventWithEmptyDescription()
    {
        // Arrange
        var veaEvent = new VeaEvent(new VeaEventId());
        
        // Assert
        Assert.Equal(veaEvent.Description.Value, string.Empty);
    }
    
    [Fact]
    public void S4_CreateEvent_WithoutEventType_ShouldCreateEventWithDefaultEventType()
    {
        // Arrange
        var veaEvent = new VeaEvent(new VeaEventId());
        var defaultEventType = VeaEventType.Private;
        
        // Assert
        Assert.Equal(veaEvent.VeaEventType, defaultEventType);
    }
}