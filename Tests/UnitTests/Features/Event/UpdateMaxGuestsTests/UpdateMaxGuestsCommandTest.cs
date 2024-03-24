using ViaEventsAssociation.Core.Application.CommandDispatching.Commands.Event;
using VIAEventsAssociation.Core.Tools.OperationResult.Error;

namespace UnitTests.Features.Event.UpdateMaxGuestsTests;

public class UpdateMaxGuestsCommandTest
{
    
    [Fact]
    public void SetMaxGuests_Success()
    {
        //Arrange
        var eventGuid = Guid.NewGuid().ToString();
        const int maxGuests = 10;
        
        //Act
        var result = UpdateMaxGuestsCommand.Create(eventGuid, maxGuests);
        
        //Assert
        Assert.False(result.IsErrorResult());
        Assert.NotNull(result.Value);
    }
    
    [Fact]
    public void SetMaxGuestsWithInvalidGuid_Fail()
    {
        //Arrange
        const string eventGuid = "123"; //invalid guid
        const int maxGuests = 10;
        
        //Act
        var result = UpdateMaxGuestsCommand.Create(eventGuid, maxGuests);
        
        //Assert
        Assert.True(result.IsErrorResult());
        Assert.Null(result.Value);
        Assert.Equal(ErrorType.ValidationFailed, result.Errors.First().Type);
    }
    
    [Fact]
    public void SetMaxGuestsWithInvalidMaxGuests_Fail()
    {
        //Arrange
        var eventGuid = Guid.NewGuid().ToString();
        const int maxGuests = 0; //invalid maxGuests
        
        //Act
        var result = UpdateMaxGuestsCommand.Create(eventGuid, maxGuests);
        
        //Assert
        Assert.True(result.IsErrorResult());
        Assert.Null(result.Value);
        Assert.Equal(ErrorType.InvalidMaxGuests, result.Errors.First().Type);
    }
}