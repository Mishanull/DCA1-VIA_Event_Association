using UnitTests.FakeServices;
using UnitTests.FakeServices.Repositories;
using ViaEventAssociation.Core.Domain.Contracts.Repositories;
using ViaEventsAssociation.Core.Application.CommandDispatching.Commands.Event;
using ViaEventsAssociation.Core.Application.CommandDispatching.Common.Base;
using ViaEventsAssociation.Core.Application.CommandDispatching.Handlers.Event;

namespace UnitTests.Features.Event.CreateEventTests;

public class CreateEventHandlerTest
{
    private VeaEventBuilder _veaEventBuilder;
    private IVeaEventRepository _veaEventRepository;
    private ICommandHandler<CreateEventCommand> _handler;
    
    [Fact]
    public async void CreateEvent_Success()
    {
        //Arrange
        Setup();
        var commandResult = CreateEventCommand.Create(new FakeCurrentTime());
        
        //Act
        var result = await _handler.HandleAsync(commandResult.Value!);
        
        //Assert
        Assert.False(result.IsErrorResult());
    }
    
    private void Setup()
    {
        _veaEventBuilder = new VeaEventBuilder();
        _veaEventRepository = new FakeVeaEventRepositoryImpl();
        _handler = new CreateEventHandler(_veaEventRepository, new FakeUow());
    }
}