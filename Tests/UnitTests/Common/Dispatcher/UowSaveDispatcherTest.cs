using Moq;
using UnitTests.FakeServices;
using ViaEventAssociation.Core.Domain.Contracts.UnitOfWork;
using ViaEventsAssociation.Core.Application.AppEntry;
using ViaEventsAssociation.Core.Application.CommandHandler.Commands.Event;
using VIAEventsAssociation.Core.Tools.OperationResult.Error;
using VIAEventsAssociation.Core.Tools.OperationResult.OperationResult;

namespace UnitTests.Common.Dispatcher;

public class UowSaveDispatcherTest
{
    [Fact]
    public async Task Dispatch_WithSuccessCommand_ShouldSaveChangesAsync()
    {
        // Arrange
        var mockDispatcher = new Mock<ICommandDispatcher>();
        var mockUnitOfWork = new Mock<IUnitOfWork>();
        var creatorId = Guid.NewGuid().ToString();
        var commandResult = CreateEventCommand.Create(creatorId, new FakeCurrentTime());
        var expectedResult = new Result();

        mockDispatcher.Setup(d => d.Dispatch(commandResult.Value!)).ReturnsAsync(expectedResult);
        mockUnitOfWork.Setup(u => u.SaveChangesAsync()).Returns(Task.CompletedTask);

        var uowSaveDispatcher = new UowSaveDispatcher(mockDispatcher.Object, mockUnitOfWork.Object);

        // Act
        var result = await uowSaveDispatcher.Dispatch(commandResult.Value!);

        // Assert
        Assert.False(result.IsErrorResult());
        mockUnitOfWork.Verify(u => u.SaveChangesAsync(), Times.Once);
    }
    
    [Fact]
    public async Task Dispatch_WithErrorCommand_ShouldNotSaveChangesAsync()
    {
        // Arrange
        var mockDispatcher = new Mock<ICommandDispatcher>();
        var mockUnitOfWork = new Mock<IUnitOfWork>();
        var creatorId = Guid.NewGuid().ToString();
        var commandResult = CreateEventCommand.Create(creatorId, new FakeCurrentTime());
        var expectedResult = new Result();
        expectedResult.CollectError(new VeaError(ErrorType.ActionNotAllowed, new ErrorMessage("Test")));

        mockDispatcher.Setup(d => d.Dispatch(commandResult.Value!)).ReturnsAsync(expectedResult);

        var uowSaveDispatcher = new UowSaveDispatcher(mockDispatcher.Object, mockUnitOfWork.Object);

        // Act
        var result = await uowSaveDispatcher.Dispatch(commandResult.Value!);

        // Assert
        Assert.True(result.IsErrorResult());
        Assert.Equal(expectedResult, result);
        mockUnitOfWork.Verify(u => u.SaveChangesAsync(), Times.Never);
    }
}