using ViaEventAssociation.Core.Domain.CreatorAgg;
using ViaEventAssociation.Core.Domain.LocationAgg;

namespace UnitTests.Features.LocationFeatures.CreateLocationTests;

public class CreateLocationAggregateTest
{
    [Fact]
    public void CreateLocation_Success_WithValidNameAndCreatorId()
    {
        // Arrange
        var validNameResult = LocationName.Create("Valid Location Name");
        Assert.False(validNameResult.IsErrorResult()); 

        var creatorId = new CreatorId();

        // Act
        var result = Location.Create(validNameResult.Value!, creatorId);

        // Assert
        Assert.False(result.IsErrorResult());
        Assert.NotNull(result.Value);
        Assert.Equal("Valid Location Name", result.Value.Name.Value);
        Assert.Equal(5, result.Value.MaxGuests.Value);
        Assert.Equal(DateTime.Today, result.Value.FromTo.Start);
        Assert.Equal(DateTime.Today.AddDays(7), result.Value.FromTo.End);
    }

    [Fact]
    public void CreateLocation_Failure_NameTooShort()
    {
        // Arrange
        var invalidName = "Shrt";

        // Act
        var result = LocationName.Create(invalidName);

        // Assert
        Assert.True(result.IsErrorResult());
        Assert.Contains(result.Errors, e => e.Message.Message.Contains("The range for the length is 5-120"));
    }

    [Fact]
    public void CreateLocation_Failure_NameTooLong()
    {
        // Arrange
        var invalidName = new string('A', 121); 

        // Act
        var result = LocationName.Create(invalidName);

        // Assert
        Assert.True(result.IsErrorResult());
        Assert.Contains(result.Errors, e => e.Message.Message.Contains("The range for the length is 5-120"));
    }
}
