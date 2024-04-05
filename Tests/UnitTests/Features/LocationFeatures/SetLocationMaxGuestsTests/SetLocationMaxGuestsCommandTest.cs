using ViaEventsAssociation.Core.Application.CommandHandler.Commands.Location;

namespace UnitTests.Features.LocationFeatures.SetLocationMaxGuestsTests;

public class SetLocationMaxGuestsCommandTest
{
    private static readonly string ValidLocationIdString = Guid.NewGuid().ToString();
    
    [Fact]
    public void SetLocationMaxGuestsCommand_Success_WithValidInputs()
    {
        // Act
        var result = SetLocationMaxGuestsCommand.Create(6, ValidLocationIdString);

        // Assert
        Assert.False(result.IsErrorResult());
        Assert.NotNull(result.Value);
        Assert.Equal(6, result.Value.MaxGuests.Value);
    }

    [Fact]
    public void UpdateLocationCommand_Failure_InvalidLocationName()
    {
        // Act
        var result = SetLocationMaxGuestsCommand.Create(-1, ValidLocationIdString);

        // Assert
        Assert.True(result.IsErrorResult());
        Assert.Null(result.Value);
    }
}