using Moq;
using ViaEventAssociation.Core.Domain.LocationAgg;
using ViaEventsAssociation.Core.Application.CommandHandler.Commands.Location;
using ViaEventsAssociation.Core.Application.CommandHandler.Common.Base;
using ViaEventsAssociation.Core.Application.CommandHandler.Handlers.LocationHandlers;
using VIAEventsAssociation.Core.Tools.OperationResult.Error;
using VIAEventsAssociation.Core.Tools.OperationResult.Helpers;
using VIAEventsAssociation.Core.Tools.OperationResult.OperationResult;

namespace UnitTests.Features.LocationFeatures.CreateLocationTests;

public class CreateLocationHandlerTest
{
    private readonly Mock<ILocationRepository> _locationRepositoryMock;
    private readonly ICommandHandler<CreateLocationCommand> _handler;
    private readonly CreateLocationCommand _validCommand;

    public CreateLocationHandlerTest()
    {
        _locationRepositoryMock = new Mock<ILocationRepository>();
        _handler = new CreateLocationHandler(_locationRepositoryMock.Object);
        _validCommand = CreateLocationCommand.Create("Valid Location Name", Guid.NewGuid().ToString()).Value!;
    }

    [Fact]
    public async Task CreateLocation_AllValuesValid_Success()
    {
        // Arrange
        _locationRepositoryMock
            .Setup(repo => repo.AddAsync(It.IsAny<ViaEventAssociation.Core.Domain.LocationAgg.Location>()))
            .ReturnsAsync(new Result());

        // Act
        var result = await _handler.HandleAsync(_validCommand);

        // Assert
        Assert.False(result.IsErrorResult());
        _locationRepositoryMock.Verify(repo => repo.AddAsync(It.IsAny<ViaEventAssociation.Core.Domain.LocationAgg.Location>()), Times.Once);
    }

    [Fact]
    public async Task CreateLocation_RepoError_Failure()
    {
        // Arrange
        var repoResult = new Result();
        repoResult.CollectError(ErrorHelper.CreateVeaError("Repository Error", ErrorType.Unknown));
        _locationRepositoryMock
            .Setup(repo => repo.AddAsync(It.IsAny<ViaEventAssociation.Core.Domain.LocationAgg.Location>()))
            .ReturnsAsync(repoResult);

        // Act
        var result = await _handler.HandleAsync(_validCommand);

        // Assert
        Assert.True(result.IsErrorResult());
        Assert.Contains(result.Errors, e => e.Message.Message.Contains("Repository Error"));
        _locationRepositoryMock.Verify(repo => repo.AddAsync(It.IsAny<ViaEventAssociation.Core.Domain.LocationAgg.Location>()), Times.Once);
    }
}
