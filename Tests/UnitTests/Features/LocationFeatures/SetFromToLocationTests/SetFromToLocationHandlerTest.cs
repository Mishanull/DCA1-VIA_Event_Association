using Moq;
using ViaEventAssociation.Core.Domain.Contracts.Repositories;
using ViaEventAssociation.Core.Domain.CreatorAgg;
using ViaEventAssociation.Core.Domain.LocationAgg;
using ViaEventsAssociation.Core.Application.CommandHandler.Commands.Location;
using ViaEventsAssociation.Core.Application.CommandHandler.Common.Base;
using ViaEventsAssociation.Core.Application.CommandHandler.Handlers.LocationHandlers;
using VIAEventsAssociation.Core.Tools.OperationResult.Error;
using VIAEventsAssociation.Core.Tools.OperationResult.Helpers;
using VIAEventsAssociation.Core.Tools.OperationResult.OperationResult;

namespace UnitTests.Features.LocationFeatures.SetFromToLocationTests;

public class SetFromToLocationHandlerTest
{

    private readonly Mock<ILocationRepository> _locationRepositoryMock;
    private readonly ICommandHandler<SetFromToLocationCommand> _handler;
    private readonly SetFromToLocationCommand _validCommand;
    private static readonly Location TestLocation = Location.Create(LocationName.Create("Valid Location Name").Value!, new CreatorId()).Value!;

    public SetFromToLocationHandlerTest()
    {
        _locationRepositoryMock = new Mock<ILocationRepository>();
        _handler = new SetFromToLocationHandler(_locationRepositoryMock.Object);
        _validCommand = SetFromToLocationCommand.Create(DateTime.Now, DateTime.Now.AddDays(3), Guid.NewGuid().ToString()).Value!;
    }

    [Fact]
    public async Task SetLocationMaxGuests_AllValuesValid_Success()
    {
        // Arrange
        _locationRepositoryMock
            .Setup(repo => repo.FindAsync(It.IsAny<LocationId>()))
            .ReturnsAsync(new Result<Location>(TestLocation));

        // Act
        var result = await _handler.HandleAsync(_validCommand);

        // Assert
        Assert.False(result.IsErrorResult());
    }

    [Fact]
    public async Task SetLocationMaxGuests_RepoError_Failure()
    {
        // Arrange
        var repoResult = new Result<Location>(null);
        repoResult.CollectError(ErrorHelper.CreateVeaError("Repository Error", ErrorType.Unknown));
        _locationRepositoryMock
            .Setup(repo => repo.FindAsync(It.IsAny<LocationId>()))
            .ReturnsAsync(repoResult);

        // Act
        var result = await _handler.HandleAsync(_validCommand);

        // Assert
        Assert.Null(repoResult.Value);
        Assert.True(result.IsErrorResult());
        Assert.Contains(result.Errors, e => e.Message.Message.Contains("Repository Error"));
        _locationRepositoryMock.Verify(repo => repo.FindAsync(It.IsAny<LocationId>()), Times.Once);
    }
}