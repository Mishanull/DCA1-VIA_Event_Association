using Moq;
using UnitTests.FakeServices.Repositories;
using ViaEventAssociation.Core.Domain.CreatorAgg.InviteEntity;
using ViaEventAssociation.Core.Domain.GuestAgg;
using ViaEventAssociation.Core.Domain.GuestAgg.Request;
using ViaEventAssociation.Core.Domain.GuestAgg.RequestEntity;
using ViaEventsAssociation.Core.Application.CommandHandler.Commands.Guest;
using ViaEventsAssociation.Core.Application.CommandHandler.Common.Base;
using ViaEventsAssociation.Core.Application.CommandHandler.Handlers.GuestHandlers;
using VIAEventsAssociation.Core.Tools.OperationResult.Error;
using VIAEventsAssociation.Core.Tools.OperationResult.Helpers;
using VIAEventsAssociation.Core.Tools.OperationResult.OperationResult;

namespace UnitTests.Features.Guest.GuestCancelsParticipationTests;

public class GuestCancelsParticipationHandlerTest
{
    private ICommandHandler<GuestCancelsEventParticipationCommand> _handler;
    private static readonly RequestId _inviteId = new RequestId();
    private GuestCancelsEventParticipationCommand _validCommand = GuestCancelsEventParticipationCommand.Create(_inviteId.Value.ToString()).Value!;

    [Fact]
    public async Task GuestAcceptsInvite_Successful()
    {
        //Arrange
        SetupSuccess();

        //Act
        var result = await _handler.HandleAsync(_validCommand);

        //Assert
        Assert.False(result.IsErrorResult());
    }

    [Fact]
    public async Task GuestAcceptsInvite_RepoError_Failure()
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
        var errorResult = new Result<Request>(null);
        var error = ErrorHelper.CreateVeaError("Not found.", ErrorType.ResourceNotFound);
        errorResult.CollectError(error);
        var repoMock = new Mock<IGuestRepository>();
        repoMock.Setup(r => r.FindRequestAsync(It.IsAny<RequestId>())).ReturnsAsync(errorResult);
        _handler = new GuestCancelsEventParticipationHandler(repoMock.Object, new FakeEventRepository());
        return error;
    }

    private void SetupSuccess()
    {
        _handler = new GuestCancelsEventParticipationHandler(new FakeGuestRepository(), new FakeEventRepository());
    }
}