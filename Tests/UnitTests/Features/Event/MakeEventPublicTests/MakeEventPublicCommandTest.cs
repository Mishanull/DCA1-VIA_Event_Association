using ViaEventsAssociation.Core.Application.CommandDispatching.Commands.Event;
using VIAEventsAssociation.Core.Tools.OperationResult.Error;

namespace UnitTests.Features.Event.MakeEventPublicTests;

public class MakeEventPublicCommandTest
{
    [Fact]
    public void MakeEventPublic_Success()
    {
        //Arrange
        var eventGuid = Guid.NewGuid().ToString();
        
        //Act
        var result = MakeEventPublicCommand.Create(eventGuid);
        
        //Assert
        Assert.False(result.IsErrorResult());
        Assert.NotNull(result.Value);
    }
    
    [Fact]
    public void MakeEventPublic_Fail()
    {
        //Arrange
        var eventGuid = "123"; //invalid guid
        
        //Act
        var result = MakeEventPublicCommand.Create(eventGuid);
        
        //Assert
        Assert.True(result.IsErrorResult());
        Assert.Null(result.Value);
        Assert.Equal(ErrorType.ValidationFailed, result.Errors.First().Type);
    }
}