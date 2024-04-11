using System.Reflection;
using Microsoft.Extensions.DependencyInjection;
using UnitTests.FakeServices;
using ViaEventsAssociation.Core.Application.AppEntry;
using ViaEventsAssociation.Core.Application.AppEntry.Exceptions;
using ViaEventsAssociation.Core.Application.AppEntry.ServiceRegistration;
using ViaEventsAssociation.Core.Application.CommandHandler.Commands.Event;
using ViaEventsAssociation.Core.Application.CommandHandler.Commands.Guest;
using ViaEventsAssociation.Core.Application.CommandHandler.Common.Base;

namespace UnitTests.Common.Dispatcher;

public class DispatcherInteractionTest
{
    [Fact]
    public async void Dispatch_WithZeroHandlersRegistered_ShouldThrowServiceNotFound()
    {
        // Arrange
        IServiceProvider serviceProvider = new ServiceCollection().BuildServiceProvider();
        var dispatcher = new CommandDispatcher(serviceProvider);
        var creatorId = Guid.NewGuid().ToString();
        var commandResult = CreateEventCommand.Create(creatorId, new FakeCurrentTime());

        // Act & Assert
        var exception = await Assert.ThrowsAsync<ServiceNotFoundException>(() => dispatcher.Dispatch(commandResult.Value!));
        Assert.NotNull(exception);
    }

    [Fact]
    public async void Dispatch_WithOneIncorrectHandlerRegistered_ShouldNotInvokeHandler()
    {
        // Arrange
        IServiceCollection serviceCollection = new ServiceCollection();
        // EmptyMockHandler needed here, otherwise GetService throws an error
        serviceCollection.AddCommandHandlers(Assembly.GetAssembly(typeof(EmptyMockHandler))!);
        IServiceProvider serviceProvider = serviceCollection.BuildServiceProvider();

        var handler = (ActivateEventMockHandler)serviceProvider.GetService<ICommandHandler<ActivateEventCommand>>()!;
        var actualHandler = (CreateEventMockHandler)serviceProvider.GetService<ICommandHandler<CreateEventCommand>>()!;

        var dispatcher = new CommandDispatcher(serviceProvider);
        var creatorId = Guid.NewGuid().ToString();
        var commandResult = CreateEventCommand.Create(creatorId, new FakeCurrentTime());

        // Act
        await dispatcher.Dispatch(commandResult.Value!);

        // Assert
        Assert.False(handler.WasCalled);
        Assert.Null(handler.LastHandledCommand);
        Assert.True(actualHandler.WasCalled);
        Assert.Equal(commandResult.Value, actualHandler.LastHandledCommand);
    }

    [Fact]
    public async void Dispatch_WithEmptyHandlerRegistered_ShouldThrow()
    {
        // Arrange
        IServiceCollection serviceCollection = new ServiceCollection();
        // ICommandHandler used here, no handlers needed
        serviceCollection.AddCommandHandlers(Assembly.GetAssembly(typeof(ICommandHandler<>))!);
        IServiceProvider serviceProvider = serviceCollection.BuildServiceProvider();

        var dispatcher = new CommandDispatcher(serviceProvider);
        var id = Guid.NewGuid().ToString();
        var commandResult = GuestAcceptsInviteCommand.Create(id);

        // Act & Assert
        var exception = await Assert.ThrowsAsync<ServiceNotFoundException>(() => dispatcher.Dispatch(commandResult.Value!));
        Assert.NotNull(exception);
    }

    [Fact]
    public async void Dispatch_WithOneCorrectHandlerRegistered_ShouldInvokeHandler()
    {
        // Arrange
        IServiceCollection serviceCollection = new ServiceCollection();
        serviceCollection.AddCommandHandlers(Assembly.GetAssembly(typeof(CreateEventMockHandler))!);
        IServiceProvider serviceProvider = serviceCollection.BuildServiceProvider();

        var handler =
            (CreateEventMockHandler)serviceProvider.GetService<ICommandHandler<CreateEventCommand>>()!;
        var dispatcher = new CommandDispatcher(serviceProvider);
        var creatorId = Guid.NewGuid().ToString();
        var commandResult = CreateEventCommand.Create(creatorId, new FakeCurrentTime());

        // Act
        await dispatcher.Dispatch(commandResult.Value!);

        // Assert
        Assert.True(handler.WasCalled);
        Assert.Equal(commandResult.Value!, handler.LastHandledCommand);
    }

    [Fact]
    public async void Dispatch_WithMultipleHandlersRegistered_IncludingCorrect_ShouldInvokeCorrectHandlerOnly()
    {
        // Arrange
        IServiceCollection serviceCollection = new ServiceCollection();
        serviceCollection.AddCommandHandlers(Assembly.GetAssembly(typeof(CreateEventMockHandler))!);
        IServiceProvider serviceProvider = serviceCollection.BuildServiceProvider();

        var correctHandler =
            (CreateEventMockHandler)serviceProvider.GetService<ICommandHandler<CreateEventCommand>>()!;
        var incorrectHandler =
            (ActivateEventMockHandler)serviceProvider.GetService<ICommandHandler<ActivateEventCommand>>()!;
        var dispatcher = new CommandDispatcher(serviceProvider);

        var creatorId = Guid.NewGuid().ToString();
        var commandResult = CreateEventCommand.Create(creatorId, new FakeCurrentTime());

        // Act
        await dispatcher.Dispatch(commandResult.Value!);

        // Assert
        Assert.True(correctHandler.WasCalled);
        Assert.Equal(commandResult.Value!, correctHandler.LastHandledCommand);
        Assert.False(incorrectHandler.WasCalled);
        Assert.Null(incorrectHandler.LastHandledCommand);
    }

    [Fact]
    public async void Dispatch_WithMultipleHandlersRegistered_ExcludingCorrect_ShouldNotInvoke()
    {
        // Arrange
        IServiceCollection serviceCollection = new ServiceCollection();
        serviceCollection.AddCommandHandlers(Assembly.GetAssembly(typeof(CreateEventMockHandler))!);
        IServiceProvider serviceProvider = serviceCollection.BuildServiceProvider();

        var incorrectHandler1 =
            (CreateEventMockHandler)serviceProvider.GetService<ICommandHandler<CreateEventCommand>>()!;
        var incorrectHandler2 =
            (ActivateEventMockHandler)serviceProvider.GetService<ICommandHandler<ActivateEventCommand>>()!;
        var dispatcher = new CommandDispatcher(serviceProvider);

        var id = Guid.NewGuid().ToString();
        var commandResult = GuestDeclinesInviteCommand.Create(id);

        // Act
        await dispatcher.Dispatch(commandResult.Value!);

        // Assert
        Assert.False(incorrectHandler1.WasCalled);
        Assert.Null(incorrectHandler1.LastHandledCommand);
        Assert.False(incorrectHandler2.WasCalled);
        Assert.Null(incorrectHandler2.LastHandledCommand);
    }


}