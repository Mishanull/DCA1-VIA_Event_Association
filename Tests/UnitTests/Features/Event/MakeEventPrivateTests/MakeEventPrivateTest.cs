using UnitTests.Utils;
using ViaEventAssociation.Core.Domain.EventAgg;
using VIAEventsAssociation.Core.Tools.OperationResult.Error;
using Xunit.Abstractions;

namespace UnitTests.Features.Event.MakeEventPrivateTests;

public class MakeEventPrivateTest(ITestOutputHelper testOutput)
{
    
    public static TheoryData<VeaEventStatus> ValidStatuses => new TheoryData<VeaEventStatus>
    {
        VeaEventStatus.Draft,
        VeaEventStatus.Ready,
    };
    
    [Theory]
    [MemberData(nameof(ValidStatuses))]
    public void S1_MakePrivate_WithValidStatusAndEventAlreadyPrivate_ShouldChangeNothing(VeaEventStatus status)
    {
        var veaEvent = new VeaEventBuilder()
            .Init()
            .WithStatus(status)
            .Build();

        veaEvent.MakePrivate();
        
        Assert.Equal(VeaEventType.Private, veaEvent.VeaEventType);
        Assert.Equal(status, veaEvent.VeaEventStatus);
    }

    [Theory]
    [MemberData(nameof(ValidStatuses))]
    public void S2_MakePrivate_WithValidStatusAndPublicEvent_ShouldMakeEventPrivate(VeaEventStatus status)
    {
        var veaEvent = new VeaEventBuilder()
            .Init().WithStatus(status)
            .WithEventType(VeaEventType.Public)
            .Build();

        veaEvent.MakePrivate();
        
        Assert.Equal(VeaEventType.Private, veaEvent.VeaEventType);
        Assert.Equal(VeaEventStatus.Draft, veaEvent.VeaEventStatus);
    }

    [Fact]
    public void F1_MakePrivate_WithActiveStatus_ShouldThrowInvalidStatusError()
    {
        var veaEvent = new VeaEventBuilder()
            .Init()
            .WithStatus(VeaEventStatus.Active)
            .Build();

        var result = veaEvent.MakePrivate();
        testOutput.WriteLine(result.Errors.First().Message.Message);
        
        Assert.True(result.IsErrorResult());
        Assert.Equal(ErrorType.InvalidStatus, result.Errors.First().Type);
        Assert.Equal("Active event cannot be set to 'private'", result.Errors.First().Message.Message);
    }

    [Fact]
    public void F2_MakePrivate_WithCancelledStatus_ShouldThrowInvalidStatusError()
    {
        var veaEvent = new VeaEventBuilder()
            .Init()
            .WithStatus(VeaEventStatus.Cancelled)
            .Build();

        var result = veaEvent.MakePrivate();
        testOutput.WriteLine(result.Errors.First().Message.Message);
        
        Assert.True(result.IsErrorResult());
        Assert.Equal(ErrorType.InvalidStatus, result.Errors.First().Type);
        Assert.Equal("Cancelled event cannot be set to 'private'", result.Errors.First().Message.Message);

    }
}