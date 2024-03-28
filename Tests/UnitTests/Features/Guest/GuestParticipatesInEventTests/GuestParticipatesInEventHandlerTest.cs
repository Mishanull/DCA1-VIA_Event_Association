using Moq;
using UnitTests.FakeServices.Repositories;
using ViaEventAssociation.Core.Domain.Contracts.Repositories;
using ViaEventAssociation.Core.Domain.EventAgg;
using ViaEventAssociation.Core.Domain.GuestAgg.Guest;
using ViaEventAssociation.Core.Domain.GuestAgg.Request;
using ViaEventAssociation.Core.Domain.GuestAgg.RequestEntity;
using ViaEventsAssociation.Core.Application.CommandDispatching.Commands.Guest;
using ViaEventsAssociation.Core.Application.CommandDispatching.Common.Base;
using ViaEventsAssociation.Core.Application.CommandDispatching.Handlers.Guest;
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
        _validCommand = GuestParticipatesInPublicEventCommand.Create(_request.EventId.Id.ToString()!, _request.GuestId.Id.ToString()!, _request.Reason).Value!;
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
        repoMock.Setup(r => r.Find(It.IsAny<GuestId>())).Returns(errorResult);
        _handler = new GuestParticipatesInPublicEventHandler(new FakeUow(), repoMock.Object, new FakeEventRepository(),new FakeRequestRepository() );
        return error;
    }

    private void SetupSuccess()
    {
        _handler = new GuestParticipatesInPublicEventHandler(new FakeUow(), new FakeGuestRepository(), new FakeEventRepository(), new FakeRequestRepository());
    }
}