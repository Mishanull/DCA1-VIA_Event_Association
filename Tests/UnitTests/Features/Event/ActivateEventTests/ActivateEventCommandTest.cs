using ViaEventsAssociation.Core.Application.CommandDispatching.Commands.Event;
using VIAEventsAssociation.Core.Tools.OperationResult.Error;

namespace UnitTests.Features.Event.ActivateEventTests;

public class ActivateEventCommandTest
{
    [Fact]
    public void ActivateEvent_Success()
    {
        //Arrange
        const string eventGuid = "f5b4b3b4-3b4b-4b3b-b4b3-b4b3b4b3b4b3"; //valid guid-string 
        
        //Act
        var result = ActivateEventCommand.Create(eventGuid);
        
        //Assert
        Assert.False(result.IsErrorResult());
        Assert.NotNull(result.Value);
    }
    
    [Fact]
    public void ActivateEvent_Fail()
    {
        //Arrange
        const string eventGuid = "123-456-789"; //invalid guid-string 
        
        //Act
        var result = ActivateEventCommand.Create(eventGuid);
        
        //Assert
        Assert.True(result.IsErrorResult());
        Assert.Equal(result.Errors.First().Type, ErrorType.ValidationFailed);
    }
}