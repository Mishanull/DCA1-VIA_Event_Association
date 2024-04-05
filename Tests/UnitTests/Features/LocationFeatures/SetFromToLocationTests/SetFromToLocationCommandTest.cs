using ViaEventAssociation.Core.Domain.Common.ValueObjects;
using ViaEventsAssociation.Core.Application.CommandHandler.Commands.Location;

namespace UnitTests.Features.LocationFeatures.SetFromToLocationTests;

public class SetFromToLocationCommandTest
{
    private static readonly string ValidLocationIdString = Guid.NewGuid().ToString();
    private static readonly FromTo ValidFromTo = FromTo.Create(DateTime.Now, DateTime.Now.AddDays(3)).Value!;
    
    [Fact]
    public void SetFromToLocationCommand_Success_WithValidInputs()
    {
        // Act
        var result = SetFromToLocationCommand.Create(ValidFromTo.Start, ValidFromTo.End, ValidLocationIdString);

        // Assert
        Assert.False(result.IsErrorResult());
        Assert.NotNull(result.Value);
        Assert.Equal(ValidFromTo, result.Value.FromTo);
    }

    [Fact]
    public void UpdateLocationCommand_Failure_InvalidLocationName()
    {
        // Act
        var result = SetFromToLocationCommand.Create(DateTime.Today.AddDays(1), DateTime.Today, ValidLocationIdString);

        // Assert
        Assert.True(result.IsErrorResult());
        Assert.Null(result.Value);
    }
}