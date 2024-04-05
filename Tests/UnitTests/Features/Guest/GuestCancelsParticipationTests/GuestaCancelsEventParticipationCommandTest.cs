using ViaEventsAssociation.Core.Application.CommandHandler.Commands.Guest;

namespace UnitTests.Features.Guest.GuestCancelsParticipationTests;

public class GuestCancelsEventParticipationCommandTest
{
    [Fact]
    public void GuestCancelsEventParticipationCommand_Success_ValidRequestId()
    {
        // Arrange
        var validRequestId = Guid.NewGuid().ToString();

        // Act
        var result = GuestCancelsEventParticipationCommand.Create(validRequestId);

        // Assert
        Assert.False(result.IsErrorResult());
        Assert.NotNull(result.Value);
        Assert.IsType<GuestCancelsEventParticipationCommand>(result.Value);
        Assert.Equal(validRequestId, result.Value!.RequestId.Id.ToString(), ignoreCase: true);
    }

    [Fact]
    public void GuestCancelsEventParticipationCommand_Failure_InvalidRequestId()
    {
        // Arrange
        var invalidRequestId = "this-is-not-a-valid-request-id";

        // Act
        var result = GuestCancelsEventParticipationCommand.Create(invalidRequestId);

        // Assert
        Assert.True(result.IsErrorResult());
        Assert.Null(result.Value);
    }
}