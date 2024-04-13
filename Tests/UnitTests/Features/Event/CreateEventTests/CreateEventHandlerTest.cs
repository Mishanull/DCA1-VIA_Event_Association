using UnitTests.FakeServices;
using UnitTests.FakeServices.Repositories;
using UnitTests.Utils;
using ViaEventAssociation.Core.Domain.EventAgg;
using ViaEventsAssociation.Core.Application.CommandHandler.Commands.Event;
using ViaEventsAssociation.Core.Application.CommandHandler.Common.Base;
using ViaEventsAssociation.Core.Application.CommandHandler.Handlers.Event;

namespace UnitTests.Features.Event.CreateEventTests;

public class CreateEventHandlerTest
{
    private IVeaEventRepository _veaEventRepository;
    private ICommandHandler<CreateEventCommand> _handler;
    
    [Fact]
    public async void CreateEvent_Success()
    {
        //Arrange
        Setup();
        var creatorId = Guid.NewGuid().ToString();
        var commandResult = CreateEventCommand.Create(creatorId, new FakeCurrentTime());
        
        //Act
        var result = await _handler.HandleAsync(commandResult.Value!);
        
        //Assert
        Assert.False(result.IsErrorResult());
    }
    
    private void Setup()
    {
        _veaEventRepository = new FakeVeaEventRepositoryImpl();
        _handler = new CreateEventHandler(_veaEventRepository, new FakeUow());
    }
}