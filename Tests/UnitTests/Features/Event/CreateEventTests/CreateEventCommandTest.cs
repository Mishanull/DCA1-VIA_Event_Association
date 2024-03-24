using UnitTests.FakeServices;
using ViaEventsAssociation.Core.Application.CommandDispatching.Commands.Event;
using VIAEventsAssociation.Core.Tools.OperationResult.Error;

namespace UnitTests.Features.Event.CreateEventTests;

public class CreateEventCommandTest
{
    [Fact]
    public void CreateEvent_Success()
    {
        //Act
        var result = CreateEventCommand.Create(new FakeCurrentTime());
        
        //Assert
        Assert.False(result.IsErrorResult());
        Assert.NotNull(result.Value);
    }
    
    //there is no way to fail this test !?
    // [Fact]
    // public void CreateEvent_Fail()
    // {
    //     //Act
    //     var result = CreateEventCommand.Create(new FakeCurrentTime());
    //     
    //     //Assert
    //     Assert.True(result.IsErrorResult());
    //     Assert.Equal(result.Errors.First().Type, ErrorType.ValidationFailed);
    // }
}