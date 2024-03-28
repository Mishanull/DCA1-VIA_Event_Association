using ViaEventsAssociation.Core.Application.CommandDispatching.Commands.Guest;

namespace UnitTests.Features.Guest.GuestDeclinesInviteTests;

public class GuestDeclinesInviteCommandTest
{
    [Fact]
    public void GuestDeclinesInviteCommand_Success_ValidInviteId()
    {
        // Arrange
        var validInviteId = Guid.NewGuid().ToString();

        // Act
        var result = GuestDeclinesInviteCommand.Create(validInviteId);

        // Assert
        Assert.False(result.IsErrorResult());
        Assert.NotNull(result.Value);
        Assert.IsType<GuestDeclinesInviteCommand>(result.Value);
        Assert.Equal(validInviteId, result.Value!.InviteId.Id.ToString(), ignoreCase: true);
    }

    [Fact]
    public void GuestDeclinesInviteCommand_Failure_InvalidInviteId()
    {
        // Arrange
        var invalidInviteId = "not-a-valid-invite-id";

        // Act
        var result = GuestDeclinesInviteCommand.Create(invalidInviteId);

        // Assert
        Assert.True(result.IsErrorResult());
        Assert.Null(result.Value);
    } 
}