using ViaEventAssociation.Core.Domain.CreatorAgg;
using ViaEventAssociation.Core.Domain.EventAgg;
using ViaEventAssociation.Core.Domain.LocationAgg;

namespace UnitTests.Features.LocationFeatures.SetLocationMaxGuestsTests;

public class SetLocationMaxGuestsAggregateTest
{
    private static readonly Location TestLocation = Location.Create(LocationName.Create("Valid Location Name").Value!, new CreatorId()).Value!;
    
    [Fact]
    public void SetLocationMaxGuests_Success_WithValidName()
    {
        // Arrange
        var maxGuestsResult = MaxGuests.Create(6);

        // Act
        TestLocation.SetMaxGuests(maxGuestsResult.Value!);

        // Assert
        Assert.Equal( 6,TestLocation.MaxGuests.Value);
        Assert.False(maxGuestsResult.IsErrorResult());
    }

    [Fact]
    public void SetLocationMaxGuests_Failure_InvalidName()
    {
        // Act
        var maxGuests = MaxGuests.Create(-1); 

        // Assert
        Assert.True(maxGuests.IsErrorResult());
    }
}