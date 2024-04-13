using Moq;
using UnitTests.FakeServices.Repositories;
using ViaEventAssociation.Core.Domain.EventAgg;
using ViaEventAssociation.Core.Domain.GuestAgg;
using ViaEventAssociation.Core.Domain.GuestAgg.Guest;
using ViaEventAssociation.Core.Domain.GuestAgg.Request;
using ViaEventAssociation.Core.Domain.GuestAgg.RequestEntity;
using ViaEventsAssociation.Core.Application.CommandHandler.Commands.Guest;
using ViaEventsAssociation.Core.Application.CommandHandler.Common.Base;
using ViaEventsAssociation.Core.Application.CommandHandler.Handlers.GuestHandlers;
using VIAEventsAssociation.Core.Tools.OperationResult.Error;
using VIAEventsAssociation.Core.Tools.OperationResult.Helpers;
using VIAEventsAssociation.Core.Tools.OperationResult.OperationResult;

namespace UnitTests.Features.Guest.GuestParticipatesInEventTests;

public class GuestParticipatesInEventHandlerTest
{
    private ICommandHandler<GuestParticipatesInPublicEventCommand> _handler;
    private static Request _request;
    private GuestParticipatesInPublicEventCommand _validCommand;

    public GuestParticipatesInEventHandlerTest()
    {
        _request = Request.Create("Interested.", new VeaEventId(), new GuestId()).Value!;
        _validCommand = GuestParticipatesInPublicEventCommand.Create(_request.EventId.Value.ToString()!, _request.GuestId.Value.ToString()!, _request.Reason).Value!;
    }

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
        var repoMock = new Mock<IGuestRepository>();
        var errorResult = new Result<VeaGuest>(null);
        var error = ErrorHelper.CreateVeaError("Not found.", ErrorType.ResourceNotFound);
        errorResult.CollectError(error);
        repoMock.Setup(r => r.FindAsync(It.IsAny<GuestId>())).ReturnsAsync(errorResult);
        _handler = new GuestParticipatesInPublicEventHandler(repoMock.Object, new FakeEventRepository());
        return error;
    }

    private void SetupSuccess()
    {
        _handler = new GuestParticipatesInPublicEventHandler(new FakeGuestRepository(), new FakeEventRepository());
    }
}