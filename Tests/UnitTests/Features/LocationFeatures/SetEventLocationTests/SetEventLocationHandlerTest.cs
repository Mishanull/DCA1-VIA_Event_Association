using Moq;
using ViaEventAssociation.Core.Domain.Contracts.Repositories;
using ViaEventAssociation.Core.Domain.CreatorAgg;
using ViaEventAssociation.Core.Domain.EventAgg;
using ViaEventAssociation.Core.Domain.LocationAgg;
using ViaEventsAssociation.Core.Application.CommandHandler.Commands.Location;
using ViaEventsAssociation.Core.Application.CommandHandler.Common.Base;
using ViaEventsAssociation.Core.Application.CommandHandler.Handlers.LocationHandlers;
using VIAEventsAssociation.Core.Tools.OperationResult.Error;
using VIAEventsAssociation.Core.Tools.OperationResult.Helpers;
using VIAEventsAssociation.Core.Tools.OperationResult.OperationResult;

namespace UnitTests.Features.LocationFeatures.SetEventLocationTests;

public class SetEventLocationHandlerTest
{
    private readonly Mock<ILocationRepository> _locationRepoMock = new Mock<ILocationRepository>();
    private readonly Mock<IVeaEventRepository> _eventRepoMock = new Mock<IVeaEventRepository>();
    private ICommandHandler<SetEventLocationCommand> _handler;
    private static Location ValidLocation = Location.Create(LocationName.Create("Initial Location Name").Value!, new CreatorId()).Value!;
    private static VeaEvent ValidEvent = new VeaEvent(new VeaEventId());
    private static SetEventLocationCommand _command = SetEventLocationCommand.Create(ValidLocation.Id.ToString(), ValidEvent.Id.ToString()).Value!;

    public SetEventLocationHandlerTest()
    {
        _handler = new SetEventLocationHandler(_eventRepoMock.Object, _locationRepoMock.Object);
    }


    private void RepoMockSetup()
    {
        _locationRepoMock.Setup(repo => repo.FindAsync(It.IsAny<LocationId>())).ReturnsAsync(new Result<Location>(ValidLocation));
        _eventRepoMock.Setup(repo => repo.FindAsync(It.IsAny<VeaEventId>())).ReturnsAsync(new Result<VeaEvent>(ValidEvent));
        _eventRepoMock.Setup(repo => repo.UpdateAsync(It.IsAny<VeaEvent>())).ReturnsAsync(new Result<VeaEvent>(ValidEvent));
    }

    [Fact]
    private async Task SetEventLocation_EventAndLocationExist_Success()
    {
        //Arrange
        RepoMockSetup();

        //Act
        var result = await _handler.HandleAsync(_command);

        //Assert
        Assert.False(result.IsErrorResult());
    }

    [Fact]
    private async Task SetEventLocation_RepoError_Failure()
    {
        //Arrange
        RepoMockSetup();
        var errorResult = new Result<VeaEvent>(null);
        errorResult.CollectError(ErrorHelper.CreateVeaError("Event not found", ErrorType.ResourceNotFound));
        _eventRepoMock.Setup(repo => repo.FindAsync(It.IsAny<VeaEventId>())).ReturnsAsync(errorResult);

        //Act
        var result = await _handler.HandleAsync(_command);

        //Assert
        Assert.True(result.IsErrorResult());
    }
}