using ViaEventAssociation.Core.Domain.Common.Base;
using ViaEventAssociation.Core.Domain.CreatorAgg;
using ViaEventsAssociation.Core.Application.CommandHandler.Commands.Location;

namespace UnitTests.Features.LocationFeatures.UpdateLocationNameTests;

public class UpdateLocationNameCommandTest
{

    [Fact]
    public void UpdateLocationNameCommand_Success_WithValidInputs()
    {
        // Arrange
        var validName = "Valid Location Name";
        var validCreatorIdString = Guid.NewGuid().ToString();
        var creatorIdResult = TId.FromString<CreatorId>(validCreatorIdString);
        Assert.False(creatorIdResult.IsErrorResult());

        // Act
        var result = UpdateLocationNameCommand.Create(validName, validCreatorIdString);

        // Assert
        Assert.False(result.IsErrorResult());
        Assert.NotNull(result.Value);
        Assert.Equal(validName, result.Value.LocationName.Value);
    }

    [Fact]
    public void UpdateLocationCommand_Failure_InvalidLocationName()
    {
        // Arrange
        var invalidName = "Shrt";
        var validCreatorIdString = Guid.NewGuid().ToString();

        // Act
        var result = UpdateLocationNameCommand.Create(invalidName, validCreatorIdString);

        // Assert
        Assert.True(result.IsErrorResult());
        Assert.Null(result.Value);
        Assert.Contains(result.Errors, e => e.Message.Message.Contains("Validation failed for location name."));
    }
}