using ViaEventsAssociation.Core.Application.CommandDispatching.Commands.Guest;
using VIAEventsAssociation.Core.Tools.OperationResult.OperationResult;

namespace UnitTests.Features.Guest.GuestAcceptsInviteTests;

public class GuestAcceptsInviteCommandTest
{
    [Fact]
    public void GuestAcceptsInviteCommand_Success_ValidGuid()
    {
        // Arrange
        var validGuid = Guid.NewGuid().ToString();

        // Act
        Result<GuestAcceptsInviteCommand> result = GuestAcceptsInviteCommand.Create(validGuid);
        GuestAcceptsInviteCommand command = result.Value!;
        
        // Assert
        Assert.False(result.IsErrorResult());
        Assert.NotNull(result.Value);
        Assert.Equal(validGuid, command.InviteId.Id.ToString(), ignoreCase: true);
    }

    [Fact]
    public void GuestAcceptsInviteCommand_Failure_InvalidGuid()
    {
        // Arrange
        var invalidGuid = "not-a-valid-guid";

        // Act
        var result = GuestAcceptsInviteCommand.Create(invalidGuid);

        // Assert
        Assert.True(result.IsErrorResult());
        Assert.Null(result.Value);
    } 
}