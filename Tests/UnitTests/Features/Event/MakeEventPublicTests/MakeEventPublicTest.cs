using UnitTests.Utils;
using ViaEventAssociation.Core.Domain.EventAgg;
using VIAEventsAssociation.Core.Tools.OperationResult.Error;
using Xunit.Abstractions;

namespace UnitTests.Features.Event.MakeEventPublicTests;

public class MakeEventPublicTest(ITestOutputHelper testOutput)
{
    public static TheoryData<VeaEventStatus> ValidStatuses => new TheoryData<VeaEventStatus>
    {
        VeaEventStatus.Draft,
        VeaEventStatus.Ready,
        VeaEventStatus.Active
    };
    
    [Theory]
    [MemberData(nameof(ValidStatuses))]
    public void S1_MakePublic_WithValidStatus_ShouldMakeEventPublic(VeaEventStatus status)
    {
        var veaEvent = new VeaEventBuilder().Init().WithStatus(status).Build();
        
        veaEvent.MakePublic();
        
        Assert.Equal(VeaEventType.Public, veaEvent.VeaEventType);
        Assert.Equal(veaEvent.VeaEventStatus, status);
    }

    [Fact]
    public void F1_MakePublic_WithInvalidStatus_ShouldThrowInvalidStatusError()
    {
        var invalidStatus = VeaEventStatus.Cancelled;
        var veaEvent = new VeaEventBuilder().Init().WithStatus(invalidStatus).Build();

        var result = veaEvent.MakePublic();
        
        Assert.True(result.IsErrorResult());
        Assert.Equal(ErrorType.InvalidStatus , result.Errors.First().Type);
        Assert.Equal("Cancelled event cannot be set to 'public'", result.Errors.First().Message.Message);
    }
}