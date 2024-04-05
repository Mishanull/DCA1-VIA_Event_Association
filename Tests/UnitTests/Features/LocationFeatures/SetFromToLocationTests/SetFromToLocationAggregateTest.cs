using ViaEventAssociation.Core.Domain.Common.ValueObjects;
using ViaEventAssociation.Core.Domain.CreatorAgg;
using ViaEventAssociation.Core.Domain.EventAgg;
using ViaEventAssociation.Core.Domain.LocationAgg;

namespace UnitTests.Features.LocationFeatures.SetFromToLocationTests;

public class SetFromToLocationAggregateTest
{
    private static readonly Location TestLocation = Location.Create(LocationName.Create("Valid Location Name").Value!, new CreatorId()).Value!;
    private static readonly FromTo ValidFromTo = FromTo.Create(DateTime.Now, DateTime.Now.AddDays(3)).Value!;
    
    [Fact]
    public void SetFromToLocation_Success_WithValidName()
    {
        // Act
        TestLocation.SetFromTo(ValidFromTo);

        // Assert
        Assert.Equal(ValidFromTo, TestLocation.FromTo);
    }

    [Fact]
    public void SetFromToLocation_Failure_InvalidName()
    {
        // Act
       var fromToCreationResult = FromTo.Create(DateTime.Today.AddDays(1), DateTime.Today);
       
       //Assert
       Assert.True(fromToCreationResult.IsErrorResult());
    }
}