using ViaEventsAssociation.Core.Application.CommandHandler.Commands.Guest;

namespace UnitTests.Features.Guest.GuestIsInvitedTests;

public class GuestIsInvitedToEventCommandTest
{
   [Fact]
    public void GuestIsInvitedToEventCommand_Success_AllIdsValid()
    {
        // Arrange
        var validCreatorId = Guid.NewGuid().ToString();
        var validGuestId = Guid.NewGuid().ToString();
        var validEventId = Guid.NewGuid().ToString();

        // Act
        var result = GuestIsInvitedToEventCommand.Create( validCreatorId, validGuestId, validEventId);

        // Assert
        Assert.False(result.IsErrorResult());
        Assert.NotNull(result.Value);
    }

    [Fact]
    public void GuestIsInvitedToEventCommand_Failure_InvalidCreatorId()
    {
        // Arrange
        var invalidCreatorId = "not-a-valid-guid";
        var validGuestId = Guid.NewGuid().ToString();
        var validEventId = Guid.NewGuid().ToString();

        // Act
        var result = GuestIsInvitedToEventCommand.Create(invalidCreatorId, validGuestId, validEventId);

        // Assert
        Assert.True(result.IsErrorResult());
        Assert.Null(result.Value);
    }

    [Fact]
    public void GuestIsInvitedToEventCommand_Failure_InvalidGuestId()
    {
        // Arrange
        var validCreatorId = Guid.NewGuid().ToString();
        var invalidGuestId = "not-a-valid-guid";
        var validEventId = Guid.NewGuid().ToString();

        // Act
        var result = GuestIsInvitedToEventCommand.Create(validCreatorId, invalidGuestId, validEventId);

        // Assert
        Assert.True(result.IsErrorResult());
        Assert.Null(result.Value);
    }

    [Fact]
    public void GuestIsInvitedToEventCommand_Failure_InvalidVeaEventId()
    {
        // Arrange
        var validCreatorId = Guid.NewGuid().ToString();
        var validGuestId = Guid.NewGuid().ToString();
        var invalidEventId = "not-a-valid-guid";

        // Act
        var result = GuestIsInvitedToEventCommand.Create(validCreatorId, validGuestId, invalidEventId);

        // Assert
        Assert.True(result.IsErrorResult());
        Assert.Null(result.Value);
    } 
}