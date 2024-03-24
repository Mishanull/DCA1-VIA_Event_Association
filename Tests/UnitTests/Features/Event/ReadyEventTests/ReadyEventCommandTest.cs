using ViaEventsAssociation.Core.Application.CommandDispatching.Commands.Event;

namespace UnitTests.Features.Event.ReadyEventTests;

public class ReadyEventCommandTest
{
    [Fact]
    public void ReadyEvent_Success()
    {
        //Arrange
        var eventGuid = Guid.NewGuid().ToString();
        
        //Act
        var result = ReadyEventCommand.Create(eventGuid);
        
        //Assert
        Assert.False(result.IsErrorResult());
        Assert.NotNull(result.Value);
    }
    
    [Fact]
    public void ReadyEvent_Fail()
    {
        //Arrange
        var eventGuid = "123"; //invalid guid
        
        //Act
        var result = ReadyEventCommand.Create(eventGuid);
        
        //Assert
        Assert.True(result.IsErrorResult());
        Assert.Null(result.Value);
    }
}