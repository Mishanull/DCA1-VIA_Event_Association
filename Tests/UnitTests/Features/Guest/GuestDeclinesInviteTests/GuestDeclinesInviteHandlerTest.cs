using Moq;
using UnitTests.FakeServices.Repositories;
using ViaEventAssociation.Core.Domain.Contracts.Repositories;
using ViaEventAssociation.Core.Domain.CreatorAgg.InviteEntity;
using ViaEventsAssociation.Core.Application.CommandDispatching.Commands.Guest;
using ViaEventsAssociation.Core.Application.CommandDispatching.Common.Base;
using ViaEventsAssociation.Core.Application.CommandDispatching.Handlers.Guest;
using VIAEventsAssociation.Core.Tools.OperationResult.Error;
using VIAEventsAssociation.Core.Tools.OperationResult.Helpers;
using VIAEventsAssociation.Core.Tools.OperationResult.OperationResult;

namespace UnitTests.Features.Guest.GuestDeclinesInviteTests;

public class GuestDeclinesInviteHandlerTest
{

    private ICommandHandler<GuestDeclinesInviteCommand> _handler;
    private static readonly InviteId _inviteId = new InviteId();
    private GuestDeclinesInviteCommand _validCommand = GuestDeclinesInviteCommand.Create(_inviteId.Id.ToString()).Value!;

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

        var repoMock = new Mock<IInviteRepository>();
        var errorResult = new Result<Invite>(null);
        var error = ErrorHelper.CreateVeaError("Not found.", ErrorType.ResourceNotFound);
        errorResult.CollectError(error);
        repoMock.Setup(r => r.Find(It.IsAny<InviteId>())).Returns(errorResult);
        _handler = new GuestDeclinesInviteHandler(new FakeGuestRepository(), new FakeEventRepository(), repoMock.Object, new FakeCreatorRepository(), new FakeUow());
        return error;
    }

    private void SetupSuccess()
    {
        _handler = new GuestDeclinesInviteHandler(new FakeGuestRepository(), new FakeEventRepository(), new FakeInviteRepository(), new FakeCreatorRepository(), new FakeUow());
    }
}