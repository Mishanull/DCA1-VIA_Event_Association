using Moq;
using UnitTests.FakeServices.Repositories;
using ViaEventAssociation.Core.Domain.Contracts.Repositories;
using ViaEventAssociation.Core.Domain.CreatorAgg;
using ViaEventAssociation.Core.Domain.CreatorAgg.InviteEntity;
using ViaEventAssociation.Core.Domain.EventAgg;
using ViaEventAssociation.Core.Domain.GuestAgg.Guest;
using ViaEventAssociation.Core.Domain.GuestAgg.RequestEntity;
using ViaEventsAssociation.Core.Application.CommandHandler.Commands.Guest;
using ViaEventsAssociation.Core.Application.CommandHandler.Common.Base;
using ViaEventsAssociation.Core.Application.CommandHandler.Handlers.GuestHandlers;
using VIAEventsAssociation.Core.Tools.OperationResult.Error;
using VIAEventsAssociation.Core.Tools.OperationResult.Helpers;
using VIAEventsAssociation.Core.Tools.OperationResult.OperationResult;

namespace UnitTests.Features.Guest.GuestIsInvitedTests;

public class GuestIsInvitedToEventHandlerTest
{
    private ICommandHandler<GuestIsInvitedToEventCommand> _handler;
    private static Invite _invite;
    private GuestIsInvitedToEventCommand _validCommand;

    public GuestIsInvitedToEventHandlerTest()
    {
        _invite = Invite.Create(new GuestId(), new CreatorId(), new VeaEventId()).Value!; 
        _validCommand = GuestIsInvitedToEventCommand.Create(_invite.CreatorId.Value.ToString()!, _invite.GuestId.Value.ToString()!, _invite.EventId.Value.ToString()).Value!;
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
        var repoMock = new Mock<ICreatorRepository>();
        var errorResult = new Result<Creator>(null);
        var error = ErrorHelper.CreateVeaError("Not found.", ErrorType.ResourceNotFound);
        errorResult.CollectError(error);
        repoMock.Setup(r => r.Find(It.IsAny<CreatorId>())).Returns(errorResult);
        _handler = new GuestIsInvitedToEventHandler(new FakeGuestRepository(), repoMock.Object, new FakeEventRepository(), new FakeInviteRepository());
        return error;
    }

    private void SetupSuccess()
    {
        _handler = new GuestIsInvitedToEventHandler(new FakeGuestRepository(), new FakeCreatorRepository(), new FakeEventRepository(), new FakeInviteRepository());
    }
}