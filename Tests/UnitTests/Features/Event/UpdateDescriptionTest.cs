using ViaEventAssociation.Core.Domain.EventAgg;
using VIAEventsAssociation.Core.Tools.OperationResult.Error;

namespace UnitTests.Features.Event;

public class UpdateDescriptionTest
{
    [Fact]
    public void S1_UpdateDescriptionOfDraftEvent_WithValidDescription_ShouldUpdateDescription()
    {
        // Arrange
        var veaEvent = new VeaEventBuilder().Init()
            .Build();
        var newDescription = Description.Create("Valid Description").Value;
        // Act
        veaEvent.UpdateDescription(newDescription);
        // Assert
        Assert.Equal(veaEvent.Description, newDescription);
    }
    
    [Fact]
    public void S2_UpdateDescription_WithEmptyDescription_ShouldUpdateDescription()
    {
        // Arrange
        var veaEvent = new VeaEventBuilder().Init()
            .WithDescription("Old valid description")
            .Build();
        // Act
        veaEvent.UpdateDescription(Description.Create().Value);
        // Assert
        Assert.Equal(veaEvent.Description, Description.Create("").Value);
    }
    
    [Fact]
    public void S3_UpdateDescriptionOfReadyEvent_WithValidDescription_ShouldUpdateDescriptionAndChangeEventStatusToDraft()
    {
        // Arrange
        var veaEvent = new VeaEventBuilder().Init()
            .WithStatus(VeaEventStatus.Ready)
            .Build();
        var newDescription = Description.Create("New valid description").Value;
        // Act
        veaEvent.UpdateDescription(newDescription);
        // Assert
        Assert.Equal(veaEvent.Description, newDescription);
        Assert.Equal(VeaEventStatus.Draft, veaEvent.VeaEventStatus);
    }
    
    [Fact]
    public void F1_UpdateDescription_WithInvalidDescription_ShouldThrowInvalidDescriptionError()
    {
        // Arrange
        var newDescriptionResult = Description.Create(
                "Nam sit amet urna tortor. Phasellus a felis semper, placerat odio nec, faucibus purus. Vestibulum ante ipsum primis in faucibus orci luctus et ultrices posuere cubilia curae; Nullam dignissim ultricies eros eget semper. Nulla at mi odio viverra fusce."
            );
        // Assert
        Assert.True(newDescriptionResult.IsErrorResult());
        Assert.Equal(newDescriptionResult.Errors.First().Type, ErrorType.InvalidDescription);
    }
    
    [Fact]
    public void F2_UpdateDescriptionOfCancelledEvent_WithValidDescription_ShouldThrowActionNotAllowedError()
    {
        // Arrange
        var veaEvent = new VeaEventBuilder().Init()
            .WithStatus(VeaEventStatus.Cancelled)
            .Build();
        var newDescription = Description.Create("New valid description").Value;
        // Act
        var result = veaEvent.UpdateDescription(newDescription);
        // Assert
        Assert.True(result.IsErrorResult());
        Assert.Equal(result.Errors.First().Type, ErrorType.ActionNotAllowed);
    }
    
    [Fact]
    public void F3_UpdateDescriptionOfActiveEvent_WithValidDescription_ShouldThrowActionNotAllowedError()
    {
        // Arrange
        var veaEvent = new VeaEventBuilder().Init()
            .WithStatus(VeaEventStatus.Active)
            .Build();
        var newDescription = Description.Create("New valid description").Value;
        // Act
        var result = veaEvent.UpdateDescription(newDescription);
        // Assert
        Assert.True(result.IsErrorResult());
        Assert.Equal(result.Errors.First().Type, ErrorType.ActionNotAllowed);
    }
}