using ViaEventAssociation.Core.Domain.EventAgg;
using VIAEventsAssociation.Core.Tools.OperationResult.Error;
using Xunit.Abstractions;

namespace UnitTests.Features.Event;

public class ReadyTest(ITestOutputHelper testOutputHelper)
{
    private static readonly DateTime FakeDateTime = new (2023, 08, 24, 12, 00, 00);
    
    [Fact]
    public void S1_Ready_WithValidEventData_ShouldReadyEvent()
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

        var result = veaEvent.Ready();
        
        Assert.Equal(VeaEventStatus.Ready, veaEvent.VeaEventStatus);
        Assert.False(result.IsErrorResult());
    }

    [Fact]
    public void F1_Ready_WithInvalidEventData_ShouldThrowError()
    {
        var veaEvent = new VeaEventBuilder()
            .Init()
            .Build();

        var result = veaEvent.Ready();
        
         foreach (var error in result.Errors)
         {
             testOutputHelper.WriteLine($"Error: {error.Type.DisplayName} - {error.Message.Message}");
         }
         
        Assert.True(result.IsErrorResult());
        Assert.Contains(result.Errors, e => Equals(e.Type, ErrorType.InvalidTitle));
        Assert.Contains(result.Errors, e => Equals(e.Type, ErrorType.InvalidDescription));
        Assert.Contains(result.Errors, e => Equals(e.Type, ErrorType.InvalidFromTo));
    }
    
    [Fact]
    public void F2_Ready_WithCancelledStatus_ShouldThrowInvalidStatusError()
    {
        var veaEvent = new VeaEventBuilder()
            .Init()
            .WithStatus(VeaEventStatus.Cancelled)
            .Build();

        var result = veaEvent.Ready();
         
        Assert.True(result.IsErrorResult());
        Assert.Equal(ErrorType.InvalidStatus, result.Errors.First().Type);
        Assert.Equal("Cancelled event cannot be readied.", result.Errors.First().Message.Message);
    }
    
    [Fact]
    public void F3_Ready_WithInvalidStartTime_ShouldThrowError()
    {
        var invalidFrom = new DateTime(2022, 3, 15, 12, 30, 0);
        var invalidTo = new DateTime(2022, 3, 15, 17, 30, 0);
        var veaEvent = new VeaEventBuilder()
            .Init()
            .WithTime(FakeDateTime)
            .WithFromTo(invalidFrom, invalidTo)
            .Build();

        var result = veaEvent.Ready();
         
        Assert.True(result.IsErrorResult());
        Assert.Equal(ErrorType.InvalidFromTo, result.Errors.First().Type);
        Assert.Equal("Event starting in the past cannot be readied.", result.Errors.First().Message.Message);
    }
    
    [Fact]
    public void F4_Ready_WithDefaultTitle_ShouldThrowInvalidTitleError()
    {
        var veaEvent = new VeaEventBuilder()
            .Init()
            .Build();

        var result = veaEvent.Ready();
         
        Assert.True(result.IsErrorResult());
        Assert.Contains(result.Errors, e => Equals(e.Type, ErrorType.InvalidTitle));
        Assert.Contains(result.Errors, e => Equals(e.Message.Message, "Title must be set and changed from the default value"));
    }
}