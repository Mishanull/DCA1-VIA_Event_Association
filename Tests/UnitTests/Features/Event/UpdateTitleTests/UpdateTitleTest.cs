using UnitTests.Utils;
using ViaEventAssociation.Core.Domain.EventAgg;
using VIAEventsAssociation.Core.Tools.OperationResult.Error;

namespace UnitTests.Features.Event.UpdateTitleTests;

public class UpdateTitleTest
{
    [Theory]
    [InlineData("Scary Movie Night!")]
    [InlineData("Graduation Gala")]
    [InlineData("VIA Hackathon")]
    public void S1_UpdateTitle_WithValidTitle_ShouldUpdateTitle(string title)
    {
        // Arrange
        var veaEvent = new VeaEventBuilder().Init()
            .Build();
        var newTitleResult = Title.Create(title);
        var newTitle = newTitleResult.Value;
        
        // Act
        veaEvent.UpdateTitle(newTitle);
        
        // Assert
        Assert.Equal(newTitle, veaEvent.Title);
    }
    
    [Theory]
    [InlineData("Scary Movie Night!")]
    [InlineData("Graduation Gala")]
    [InlineData("VIA Hackathon")]
    public void S2_UpdateTitleOfReadyEvent_WithValidTitle_ShouldUpdateTitleAndChangeEventStatusToDraft(string title)
    {
        // Arrange
        var veaEvent = new VeaEventBuilder().Init()
            .WithStatus(VeaEventStatus.Ready)
            .Build();
        var newTitleResult = Title.Create(title);
        var newTitle = newTitleResult.Value;
        
        // Act
        veaEvent.UpdateTitle(newTitle);
        
        // Assert
        Assert.Equal(newTitle, veaEvent.Title);
        Assert.Equal(VeaEventStatus.Draft, veaEvent.VeaEventStatus);
    }
    
    [Fact]
    public void F1_UpdateTitle_WithEmptyTitle_ShouldThrowError()
    {
        // Arrange
        var newTitleResult = Title.Create("");
        
        // Assert
        Assert.True(newTitleResult.IsErrorResult());
        Assert.Equal(newTitleResult.Errors.First().Type, ErrorType.InvalidTitle);
        Assert.Equal("Title length must be between 3 and 75 characters", newTitleResult.Errors.First().Message.Message);
    }
    
    [Theory]
    [InlineData("XY")]
    [InlineData("a")]
    public void F2_UpdateTitle_WithTooShortTitle_ShouldThrowError(string? title)
    {
        // Arrange
        var newTitleResult = Title.Create(title);
        
        // Assert
        Assert.True(newTitleResult.IsErrorResult());
        Assert.Equal(newTitleResult.Errors.First().Type, ErrorType.InvalidTitle);
    }
    
    [Theory]
    [MemberData(nameof(GetTooLongTitles))]
    public void F3_UpdateTitle_WithTooLongTitle_ShouldThrowError(string? title)
    {
        // Arrange
        var newTitleResult = Title.Create(title);
        
        // Assert
        Assert.True(newTitleResult.IsErrorResult());
        Assert.Equal(newTitleResult.Errors.First().Type, ErrorType.InvalidTitle);
    }
    public static IEnumerable<object[]> GetTooLongTitles()
    {
        //loop that generates 5 random titles with 76 characters
        yield return new object[] {new string('a', 76)};
        yield return new object[] {new string('b', 76)};
        yield return new object[] {new string('y', 76)};
    }
    
    [Fact]
    public void F4_UpdateTitle_WithNullTitle_ShouldThrowError()
    {
        // Arrange
        var newTitleResult = Title.Create(null);
        
        // Assert
        Assert.True(newTitleResult.IsErrorResult());
        Assert.Equal(newTitleResult.Errors.First().Type, ErrorType.InvalidTitle);
    }
    
    [Fact]
    public void F5_UpdateTitleOfActiveEvent_WithValidTitle_ShouldThrowError()
    {
        // Arrange
        var veaEvent = new VeaEventBuilder().Init()
            .WithStatus(VeaEventStatus.Active)
            .Build();
        var newTitleResult = Title.Create("Valid title");
        var newTitle = newTitleResult.Value;
        
        // Act
        var updateResult = veaEvent.UpdateTitle(newTitle);
        
        // Assert
        Assert.Equal(updateResult.Errors.First().Type, ErrorType.ActionNotAllowed);
    }
    
    [Fact]
    public void F6_UpdateTitle_OfCanceledEvent_ShouldThrowError()
    {
        // Arrange
        var veaEvent = new VeaEventBuilder().Init()
            .WithStatus(VeaEventStatus.Cancelled)
            .Build();
        var newTitleResult = Title.Create("Valid title");
        var newTitle = newTitleResult.Value;
        
        // Act
        var updateResult = veaEvent.UpdateTitle(newTitle);
        
        // Assert
        Assert.Equal(updateResult.Errors.First().Type, ErrorType.ActionNotAllowed);
    }
}