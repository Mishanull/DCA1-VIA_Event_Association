using ViaEventsAssociation.Core.Application.CommandHandler.Commands.Event;
using VIAEventsAssociation.Core.Tools.OperationResult.Error;

namespace UnitTests.Features.Event.UpdateDescriptionTests;

public class UpdateDescriptionCommandTest
{
    [Fact]
    public void UpdateDescription_Success()
    {
        //Arrange
        var eventGuid = Guid.NewGuid().ToString();
        const string description = "Test Description";
        
        //Act
        var result = UpdateDescriptionCommand.Create(eventGuid, description);
        
        //Assert
        Assert.False(result.IsErrorResult());
        Assert.NotNull(result.Value);
        Assert.Equal(description, result.Value.Description.Value);
    }
    
    [Fact]
    public void UpdateDescription_Fail()
    {
        //Arrange
        var eventGuid = Guid.NewGuid().ToString();
        var description = new string('a', 251); //invalid description
        
        //Act
        var result = UpdateDescriptionCommand.Create(eventGuid, description);
        
        //Assert
        Assert.True(result.IsErrorResult());
        Assert.Equal(ErrorType.InvalidDescription, result.Errors.First().Type);
    }
}