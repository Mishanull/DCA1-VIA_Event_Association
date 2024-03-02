using ViaEventAssociation.Core.Domain.EventAgg;
using VIAEventsAssociation.Core.Tools.OperationResult.Error;
using VIAEventsAssociation.Core.Tools.OperationResult.OperationResult;
using Xunit.Abstractions;

namespace UnitTests.Features.Event;


public class SetMaxGuestsTest(ITestOutputHelper testOutputHelper)
{
    private readonly ITestOutputHelper _testOutputHelper = testOutputHelper;

    public static TheoryData<int, VeaEventStatus> ValidData => new TheoryData<int, VeaEventStatus>
    {
        { 5, VeaEventStatus.Draft },
        { 10, VeaEventStatus.Draft },
        { 25, VeaEventStatus.Draft },
        { 50, VeaEventStatus.Draft },
        { 5, VeaEventStatus.Ready },
        { 10, VeaEventStatus.Ready },
        { 25, VeaEventStatus.Ready },
        { 50, VeaEventStatus.Ready },
    };
    
    [Theory]
    [MemberData(nameof(ValidData))]
    public void S1_S2_SetMaxGuests_WithValidNumberAndStatus_ShouldSetMaxGuests(int numberOfGuests, VeaEventStatus status)
    {
        var veaEvent = new VeaEventBuilder()
            .Init()
            .WithStatus(status)
            .Build();
        var validMaxGuests = ((Result<MaxGuests>)MaxGuests.Create(numberOfGuests)).Value;
        
        veaEvent.SetMaxGuests(validMaxGuests);
        
        Assert.Equal(numberOfGuests, veaEvent.MaxGuests.Value);
    }

    [Theory]
    [InlineData(5)]
    [InlineData(10)]
    [InlineData(25)]
    [InlineData(50)]
    public void S3_SetMaxGuests_WithHigherNumberAndActiveStatus_ShouldSetMaxGuests(int numberOfGuests)
    {
        var veaEvent = new VeaEventBuilder()
            .Init()
            .WithStatus(VeaEventStatus.Active)
            .Build();
        var validMaxGuests = ((Result<MaxGuests>)MaxGuests.Create(numberOfGuests)).Value;

        veaEvent.SetMaxGuests(validMaxGuests);
        
        Assert.Equal(numberOfGuests, veaEvent.MaxGuests.Value);
    }

    [Fact]
    public void F1_SetMaxGuests_WithLowerNumberAndActiveStatus_ShouldThrowInvalidMaxGuestsError()
    {
        const int initialMaxGuests = 50;
        const int lowerMaxGuests = 25;
        var veaEvent = new VeaEventBuilder()
            .Init()
            .WithStatus(VeaEventStatus.Active)
            .WithMaxGuests(initialMaxGuests)
            .Build();
        var invalidMaxGuests = ((Result<MaxGuests>)MaxGuests.Create(lowerMaxGuests)).Value;
        
        var result = veaEvent.SetMaxGuests(invalidMaxGuests);
        
        Assert.True(result.IsErrorResult());
        Assert.Equal(initialMaxGuests, veaEvent.MaxGuests.Value);
        Assert.Equal(ErrorType.InvalidMaxGuests, result.Errors.First().Type);
        Assert.Equal("Maximum number of guests of an active event cannot be reduced", result.Errors.First().Message.Message);

    }

    [Fact]
    public void F2_SetMaxGuests_WithCancelledStatus_ShouldThrowInvalidMaxGuestsError()
    {
        const int maxGuestsNumber = 25;
        var veaEvent = new VeaEventBuilder()
            .Init()
            .WithStatus(VeaEventStatus.Cancelled)
            .Build();
        var maxGuests = ((Result<MaxGuests>)MaxGuests.Create(maxGuestsNumber)).Value;
        
        var result = veaEvent.SetMaxGuests(maxGuests);
        
        Assert.True(result.IsErrorResult());
        Assert.Equal(ErrorType.InvalidMaxGuests, result.Errors.First().Type);
        Assert.Equal("Cancelled event cannot be modified", result.Errors.First().Message.Message);
    }
    
    [Fact]
    public void F3_SetMaxGuests_WithMaxGuestsMoreThanLocationCapacity_ShouldThrowInvalidMaxGuestsError()
    {
        // TODO: Placeholder waiting for location implementation
    }
    
    [Fact]
    public void F4_SetMaxGuests_WithGuestNumberTooLow_ShouldThrowInvalidMaxGuestsError()
    {
        const int invalidGuestNumber = 4;
        
        var result = MaxGuests.Create(invalidGuestNumber);
        
        Assert.True(result.IsErrorResult());
        Assert.Equal(ErrorType.InvalidMaxGuests, result.Errors.First().Type);
    }
    
    [Fact]
    public void F5_SetMaxGuests_WithGuestNumberTooHigh_ShouldThrowInvalidMaxGuestsError()
    {
        const int invalidGuestNumber = 55;
        
        var result = MaxGuests.Create(invalidGuestNumber);
        
        Assert.True(result.IsErrorResult());
        Assert.Equal(ErrorType.InvalidMaxGuests, result.Errors.First().Type);
    }
    
}