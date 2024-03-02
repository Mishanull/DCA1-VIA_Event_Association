using ViaEventAssociation.Core.Domain.EventAgg;
using VIAEventsAssociation.Core.Tools.OperationResult.Error;
using Xunit.Abstractions;

namespace UnitTests.Features.Event;

public class ActivateTest(ITestOutputHelper testOutputHelper)
{
    [Fact]
    public void S1_Activate_WithValidStatusAndEventData_ShouldActivateEvent()
    {
        const string validTitle = "A Valid Title";
        const string validDescription = "A Valid Description";
        var validFrom = new DateTime(2024, 3, 15, 12, 30, 0);
        var validTo = new DateTime(2024, 3, 15, 17, 30, 0);
        var veaEvent = new VeaEventBuilder()
            .Init()
            .WithTitle(validTitle)
            .WithDescription(validDescription)
            .WithFromTo(validFrom, validTo)
            .WithEventType(VeaEventType.Public)
            .WithStatus(VeaEventStatus.Draft)
            .WithMaxGuests(25)
            .Build();

        var result = veaEvent.Activate();
        
        Assert.False(result.IsErrorResult());
        Assert.Equal(VeaEventStatus.Active, veaEvent.VeaEventStatus);
    }

    [Fact]
    public void S2_Activate_WithReadyStatus_ShouldActivateEvent()
    {
        var veaEvent = new VeaEventBuilder()
            .Init()
            .WithStatus(VeaEventStatus.Ready)
            .Build();

        var result = veaEvent.Activate();
        
        Assert.False(result.IsErrorResult());
        Assert.Equal(VeaEventStatus.Active, veaEvent.VeaEventStatus);
    }
    
    [Fact]
    public void S3_Activate_WithActiveStatus_ShouldChangeNothing()
    {
        var veaEvent = new VeaEventBuilder()
            .Init()
            .WithStatus(VeaEventStatus.Active)
            .Build();

        var result = veaEvent.Activate();
        
        Assert.False(result.IsErrorResult());
        Assert.Equal(VeaEventStatus.Active, veaEvent.VeaEventStatus);
    }

    [Fact]
    public void F1_Activate_WithInvalidEventData_ShouldThrowError()
    {
        var veaEvent = new VeaEventBuilder()
            .Init()
            .WithStatus(VeaEventStatus.Draft)
            .Build();

        var result = veaEvent.Activate();
        
        Assert.True(result.IsErrorResult());
        Assert.Contains(result.Errors, e => Equals(e.Type, ErrorType.InvalidTitle));
        Assert.Contains(result.Errors, e => Equals(e.Type, ErrorType.InvalidDescription));
        Assert.Contains(result.Errors, e => Equals(e.Type, ErrorType.InvalidFromTo));
        foreach (var error in result.Errors)
        {
            testOutputHelper.WriteLine($"{error.Type} - {error.Message.Message}");
        }
    }
    
    [Fact]
    public void F2_Activate_WithCancelledStatus_ShouldThrowInvalidStatusError()
    {
        var veaEvent = new VeaEventBuilder()
            .Init()
            .WithStatus(VeaEventStatus.Cancelled)
            .Build();

        var result = veaEvent.Activate();
        
        Assert.True(result.IsErrorResult());
        Assert.Equal(ErrorType.InvalidStatus, result.Errors.First().Type);
        Assert.Equal("Cancelled event cannot be activated", result.Errors.First().Message.Message);
    }
}