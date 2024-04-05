using ViaEventAssociation.Core.Domain.CreatorAgg;
using ViaEventAssociation.Core.Domain.LocationAgg;

namespace UnitTests.Features.LocationFeatures.UpdateLocationNameTests;

public class UpdateLocationNameAggregateTest
{
    [Fact]
    public void UpdateLocationName_Success_WithValidName()
    {
        // Arrange
        var location = Location.Create(LocationName.Create("Initial Location Name").Value!, new CreatorId()).Value!;
        var newNameResult = LocationName.Create("New Valid Location Name");
        Assert.False(newNameResult.IsErrorResult());

        // Act
        location.UpdateName(newNameResult.Value!);

        // Assert
        Assert.Equal("New Valid Location Name", location.Name.Value);
    }

    [Fact]
    public void UpdateLocationName_Failure_InvalidName()
    {
        // Act
        var newName = LocationName.Create("Shrt"); // Name is too short
        
        // Assert
        Assert.True(newName.IsErrorResult());
    }
}
