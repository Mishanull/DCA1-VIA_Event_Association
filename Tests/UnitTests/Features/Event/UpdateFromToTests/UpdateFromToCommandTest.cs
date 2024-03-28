using ViaEventsAssociation.Core.Application.CommandDispatching.Commands.Event;

namespace UnitTests.Features.Event.UpdateFromToTests;

public class UpdateFromToCommandTest
{
    [Fact]
    public void UpdateFromTo_Success()
    {
        //Arrange
        var eventGuid = Guid.NewGuid().ToString();
        var from = DateTime.Now;
        var to = DateTime.Now.AddHours(1);
        
        //Act
        var result = UpdateFromToCommand.Create(eventGuid, from, to);
        
        //Assert
        Assert.False(result.IsErrorResult());
        Assert.NotNull(result.Value);
    }
    
    [Fact]
    public void UpdateFromTo_Fail()
    {
        //Arrange
        var eventGuid = Guid.NewGuid().ToString();
        var from = DateTime.Now;
        var to = DateTime.Now.AddHours(-1);
        
        //Act
        var result = UpdateFromToCommand.Create(eventGuid, from, to);
        
        //Assert
        Assert.True(result.IsErrorResult());
    }
}