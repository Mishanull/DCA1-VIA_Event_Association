using UnitTests.FakeServices;
using ViaEventsAssociation.Core.Application.CommandHandler.Commands.Event;
using VIAEventsAssociation.Core.Tools.OperationResult.Error;

namespace UnitTests.Features.Event.CreateEventTests;

public class CreateEventCommandTest
{
    [Fact]
    public void CreateEvent_Success()
    {
        //Arrange
        var creatorId = Guid.NewGuid().ToString();
        
        //Act
        var result = CreateEventCommand.Create(creatorId, new FakeCurrentTime());
        
        //Assert
        Assert.False(result.IsErrorResult());
        Assert.NotNull(result.Value);
    }
    
    [Fact]
    public void CreateEvent_Fail()
    {
        //Arrange
        var creatorId = "123"; // Invalid creatorId
        
        //Act
        var result = CreateEventCommand.Create(creatorId, new FakeCurrentTime());
        
        //Assert
        Assert.True(result.IsErrorResult());
        Assert.Equal(result.Errors.First().Type, ErrorType.ValidationFailed);
    }
}