using Moq;
using UnitTests.FakeServices.Repositories;
using ViaEventAssociation.Core.Domain.Contracts.Repositories;
using ViaEventAssociation.Core.Domain.CreatorAgg.InviteEntity;
using ViaEventAssociation.Core.Domain.GuestAgg.Request;
using ViaEventAssociation.Core.Domain.GuestAgg.RequestEntity;
using ViaEventsAssociation.Core.Application.CommandDispatching.Commands.Guest;
using ViaEventsAssociation.Core.Application.CommandDispatching.Common.Base;
using ViaEventsAssociation.Core.Application.CommandDispatching.Handlers.Guest;
using VIAEventsAssociation.Core.Tools.OperationResult.Error;
using VIAEventsAssociation.Core.Tools.OperationResult.Helpers;
using VIAEventsAssociation.Core.Tools.OperationResult.OperationResult;

namespace UnitTests.Features.Guest.GuestCancelsParticipationTests;

public class GuestCancelsParticipationHandlerTest
{
    private ICommandHandler<GuestCancelsEventParticipationCommand> _handler;
    private static readonly RequestId _inviteId = new RequestId();
    private GuestCancelsEventParticipationCommand _validCommand = GuestCancelsEventParticipationCommand.Create(_inviteId.Id.ToString()).Value!;

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
        var repoMock = new Mock<IRequestRepository>();
        var errorResult = new Result<Request>(null);
        var error = ErrorHelper.CreateVeaError("Not found.", ErrorType.ResourceNotFound);
        errorResult.CollectError(error);
        repoMock.Setup(r => r.Find(It.IsAny<RequestId>())).Returns(errorResult);
        _handler = new GuestCancelsEventParticipationHandler(new FakeUow(), new FakeGuestRepository(), new FakeEventRepository(), repoMock.Object);
        return error;
    }

    private void SetupSuccess()
    {
        _handler = new GuestCancelsEventParticipationHandler(new FakeUow(), new FakeGuestRepository(), new FakeEventRepository(), new FakeRequestRepository());
    }
}