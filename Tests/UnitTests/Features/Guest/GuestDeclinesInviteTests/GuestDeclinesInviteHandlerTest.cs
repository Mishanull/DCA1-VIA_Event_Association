using Moq;
using UnitTests.FakeServices.Repositories;
using ViaEventAssociation.Core.Domain.Contracts.Repositories;
using ViaEventAssociation.Core.Domain.CreatorAgg.InviteEntity;
using ViaEventsAssociation.Core.Application.CommandHandler.Commands.Guest;
using ViaEventsAssociation.Core.Application.CommandHandler.Common.Base;
using ViaEventsAssociation.Core.Application.CommandHandler.Handlers.GuestHandlers;
using VIAEventsAssociation.Core.Tools.OperationResult.Error;
using VIAEventsAssociation.Core.Tools.OperationResult.Helpers;
using VIAEventsAssociation.Core.Tools.OperationResult.OperationResult;

namespace UnitTests.Features.Guest.GuestDeclinesInviteTests;

public class GuestDeclinesInviteHandlerTest
{

    private ICommandHandler<GuestDeclinesInviteCommand> _handler;
    private static readonly InviteId _inviteId = new InviteId();
    private GuestDeclinesInviteCommand _validCommand = GuestDeclinesInviteCommand.Create(_inviteId.Value.ToString()).Value!;

    [Fact]
    public async Task GuestDeclinesInvite_Successful()
    {
        //Arrange
        SetupSuccess();

        //Act
        var result = await _handler.HandleAsync(_validCommand);

        //Assert
        Assert.False(result.IsErrorResult());
    }

    [Fact]
    public async Task GuestDeclinesInvite_RepoError_Failure()
    {
        //Arrange
        var error = SetupFailure();

        //Act
        var result = await _handler.HandleAsync(_validCommand);

        //Assert
        Assert.True(result.IsErrorResult());
        Assert.Equal(result.Errors.First(), error);
    }

    private VeaError SetupFailure()
    {

        var errorResult = new Result<Invite>(null);
        var error = ErrorHelper.CreateVeaError("Not found.", ErrorType.ResourceNotFound);
        errorResult.CollectError(error);
        var creatorRepoFailMock = new Mock<ICreatorRepository>();
        creatorRepoFailMock.Setup(r => r.FindInviteAsync(It.IsAny<InviteId>())).ReturnsAsync(errorResult);
        _handler = new GuestDeclinesInviteHandler(new FakeGuestRepository(), new FakeEventRepository(),  creatorRepoFailMock.Object);
        return error;
    }

    private void SetupSuccess()
    {
        _handler = new GuestDeclinesInviteHandler(new FakeGuestRepository(), new FakeEventRepository(),  new FakeCreatorRepository());
    }
}