using ViaEventsAssociation.Core.Application.CommandHandler.Commands.Location;

namespace UnitTests.Features.LocationFeatures.SetEventLocationTests;

public class SetEventLocationCommandTest
{
    
    private static readonly string ValidLocationIdString = Guid.NewGuid().ToString();
    private static readonly string ValidEventIdString = Guid.NewGuid().ToString();
    
    [Fact]
    public void SetEventLocationCommand_Success_WithValidInputs()
    {
        // Act
        var result = SetEventLocationCommand.Create(ValidEventIdString,  ValidLocationIdString);

        // Assert
        Assert.False(result.IsErrorResult());
        Assert.NotNull(result.Value);
    }

    [Fact]
    public void SetEventLocationCommand_Failure_InvalidLocationId()
    {
        // Act
        var result = SetEventLocationCommand.Create(ValidEventIdString, "sds-232-sd2");

        // Assert
        Assert.True(result.IsErrorResult());
        Assert.Null(result.Value);

    }
    
    [Fact]
    public void SetEventLocationCommand_Failure_InvalidEventId()
    {
        // Act
        var result = SetEventLocationCommand.Create(ValidEventIdString, "sds-232-sd2");

        // Assert
        Assert.True(result.IsErrorResult());
        Assert.Null(result.Value);
    }
}