using ViaEventsAssociation.Core.Application.CommandHandler.Commands.Guest;

namespace UnitTests.Features.Guest.GuestParticipatesInEventTests;

public class GuestParticipatesInEventCommandTest
{
    [Fact]
    public void GuestParticipatesInPublicEventCommand_Success()
    {
        // Arrange
        var validEventId = Guid.NewGuid().ToString();
        var validGuestId = Guid.NewGuid().ToString();
        var reason = "Interested in the event topic.";

        // Act
        var result = GuestParticipatesInPublicEventCommand.Create(validEventId, validGuestId, reason);

        // Assert
        Assert.False(result.IsErrorResult());
        Assert.NotNull(result.Value);
        Assert.Equal(validEventId, result.Value!.EventId.Id.ToString(), ignoreCase: true);
        Assert.Equal(validGuestId, result.Value!.VeaGuestId.Id.ToString(), ignoreCase: true);
        Assert.Equal(reason, result.Value!.Reason);
    }

    [Fact]
    public void GuestParticipatesInPublicEventCommand_Failure_InvalidEventId()
    {
        // Arrange
        var invalidEventId = "invalid-event-id";
        var validGuestId = Guid.NewGuid().ToString();
        var reason = "Interested, but provided an invalid event ID.";

        // Act
        var result = GuestParticipatesInPublicEventCommand.Create(invalidEventId, validGuestId, reason);

        // Assert
        Assert.True(result.IsErrorResult());
        Assert.Null(result.Value);
    }

    [Fact]
    public void GuestParticipatesInPublicEventCommand_Failure_InvalidGuestId()
    {
        // Arrange
        var validEventId = Guid.NewGuid().ToString();
        var invalidGuestId = "invalid-guest-id";
        var reason = "Interested, but provided an invalid guest ID.";

        // Act
        var result = GuestParticipatesInPublicEventCommand.Create(validEventId, invalidGuestId, reason);

        // Assert
        Assert.True(result.IsErrorResult());
        Assert.Null(result.Value);
    } 
}