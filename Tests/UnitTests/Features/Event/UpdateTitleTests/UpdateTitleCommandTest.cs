using ViaEventsAssociation.Core.Application.CommandDispatching.Commands.Event;
using VIAEventsAssociation.Core.Tools.OperationResult.Error;

namespace UnitTests.Features.Event.UpdateTitleTests;

public class UpdateTitleCommandTest
{
    [Fact]
    public void UpdateTitle_Success()
    {
        //Arrange
        var eventGuid = Guid.NewGuid().ToString();
        const string newTitle = "New valid Title";
        
        //Act
        var result = UpdateTitleCommand.Create(eventGuid, newTitle);
        
        //Assert
        Assert.False(result.IsErrorResult());
        Assert.NotNull(result.Value);
    }
    
    [Fact]
    public void UpdateTitle_Fail()
    {
        //Arrange
        var eventGuid = Guid.NewGuid().ToString();
        const string newTitle = "Ab"; //Title is too short
        
        //Act
        var result = UpdateTitleCommand.Create(eventGuid, newTitle);
        
        //Assert
        Assert.True(result.IsErrorResult());
        Assert.Equal(ErrorType.InvalidTitle, result.Errors.First().Type);
    }
}