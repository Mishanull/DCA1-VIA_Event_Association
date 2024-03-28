using ViaEventsAssociation.Core.Application.CommandDispatching.Commands.Event;

namespace UnitTests.Features.Event.MakeEventPrivateTests;

public class MakeEventPrivateCommandTest
{
    [Fact]
    public void MakeEventPrivate_Success()
    {
        //Arrange
        var eventGuid = Guid.NewGuid().ToString();
        
        //Act
        var result = MakeEventPrivateCommand.Create(eventGuid);
        
        //Assert
        Assert.False(result.IsErrorResult());
        Assert.NotNull(result.Value);
    }
    
    [Fact]
    public void MakeEventPrivate_Fail()
    {
        //Arrange
        var eventGuid = "123"; //invalid guid
        
        //Act
        var result = MakeEventPrivateCommand.Create(eventGuid);
        
        //Assert
        Assert.True(result.IsErrorResult());
        Assert.Null(result.Value);
    }
}