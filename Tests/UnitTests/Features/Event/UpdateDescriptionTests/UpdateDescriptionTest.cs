using UnitTests.Utils;
using ViaEventAssociation.Core.Domain.EventAgg;
using VIAEventsAssociation.Core.Tools.OperationResult.Error;

namespace UnitTests.Features.Event.UpdateDescriptionTests;

public class UpdateDescriptionTest
{
    [Fact]
    public void S1_UpdateDescriptionOfDraftEvent_WithValidDescription_ShouldUpdateDescription()
    {
        // Arrange
        var veaEvent = new VeaEventBuilder().Init().Build();
        var newDescriptionResul = Description.Create("Valid Description");
        // Act
        veaEvent.UpdateDescription(newDescriptionResul.Value!);
        // Assert
        Assert.Equal(veaEvent.Description, newDescriptionResul.Value!);
    }
    
    [Fact]
    public void S2_UpdateDescription_WithEmptyDescription_ShouldUpdateDescription()
    {
        // Arrange
        var veaEvent = new VeaEventBuilder().Init()
            .WithDescription("Old valid description")
            .Build();
        // Act
        veaEvent.UpdateDescription(Description.Create().Value!);
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
        var newDescriptionResult = Description.Create("New valid description");
        // Act
        veaEvent.UpdateDescription(newDescriptionResult.Value!);
        // Assert
        Assert.Equal(veaEvent.Description, newDescriptionResult.Value);
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
        var newDescriptionResult = Description.Create("New valid description");
        // Act
        var result = veaEvent.UpdateDescription(newDescriptionResult.Value!);
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
        var newDescriptionResult = Description.Create("New valid description");
        // Act
        var result = veaEvent.UpdateDescription(newDescriptionResult.Value!);
        // Assert
        Assert.True(result.IsErrorResult());
        Assert.Equal(result.Errors.First().Type, ErrorType.ActionNotAllowed);
    }
}