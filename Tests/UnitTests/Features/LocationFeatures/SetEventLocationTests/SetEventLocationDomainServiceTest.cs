using Moq;
using ViaEventAssociation.Core.Domain.Contracts.Repositories;
using ViaEventAssociation.Core.Domain.CreatorAgg;
using ViaEventAssociation.Core.Domain.EventAgg;
using ViaEventAssociation.Core.Domain.LocationAgg;
using ViaEventAssociation.Core.Domain.Services.Location;
using ViaEventsAssociation.Core.Application.CommandHandler.Commands.Location;
using VIAEventsAssociation.Core.Tools.OperationResult.Error;
using VIAEventsAssociation.Core.Tools.OperationResult.Helpers;
using VIAEventsAssociation.Core.Tools.OperationResult.OperationResult;

namespace UnitTests.Features.LocationFeatures.SetEventLocationTests;

public class SetEventLocationDomainServiceTest
{
    private readonly Mock<ILocationRepository> _locationRepoMock = new Mock<ILocationRepository>();
    private readonly Mock<IVeaEventRepository> _eventRepoMock = new Mock<IVeaEventRepository>();
    private AddLocationToEvent _addLocationToEvent;
    private static Location ValidLocation = Location.Create(LocationName.Create("Initial Location Name").Value!, new CreatorId()).Value!;
    private static VeaEvent ValidEvent = new VeaEvent(new VeaEventId());
    
    public SetEventLocationDomainServiceTest()
    {
        _addLocationToEvent = new AddLocationToEvent(_locationRepoMock.Object, _eventRepoMock.Object);
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
        var result = await _addLocationToEvent.Handle(ValidLocation.Id, ValidEvent.Id);

        //Assert
        Assert.False(result.IsErrorResult());
    }

    [Fact]
    private async Task SetEventLocation_EventDoesNotExist_Failure()
    {
        //Arrange
        RepoMockSetup();
        var errorResult = new Result<VeaEvent>(null);
        errorResult.CollectError(ErrorHelper.CreateVeaError("Event not found", ErrorType.ResourceNotFound));
        _eventRepoMock.Setup(repo => repo.FindAsync(It.IsAny<VeaEventId>())).ReturnsAsync(errorResult);
        
        //Act
        var result = await _addLocationToEvent.Handle(ValidLocation.Id, ValidEvent.Id);
        
        //Assert
        Assert.True(result.IsErrorResult());
    }
}