using UnitTests.FakeServices;
using UnitTests.FakeServices.Repositories;
using UnitTests.Utils;
using ViaEventAssociation.Core.Domain.Contracts.Repositories;
using ViaEventAssociation.Core.Domain.EventAgg;
using ViaEventsAssociation.Core.Application.CommandHandler.Commands.Event;
using ViaEventsAssociation.Core.Application.CommandHandler.Common.Base;
using ViaEventsAssociation.Core.Application.CommandHandler.Handlers.Event;

namespace UnitTests.Features.Event.MakeEventPrivateTests;

public class MakeEventPrivateHandlerTest
{
    private VeaEventBuilder _veaEventBuilder;
    private IVeaEventRepository _veaEventRepository;
    private ICommandHandler<MakeEventPrivateCommand> _handler;
    
    [Fact]
    public async void MakeEventPrivate_Success()
    {
        //Arrange
        Setup();
        //create a public event
        var veaEvent = _veaEventBuilder.Init()
            .WithEventType(VeaEventType.Public)
            .Build();
        //add the event to the repository
        await _veaEventRepository.AddAsync(veaEvent);
        //create a command to make the event private
        var commandResult = MakeEventPrivateCommand.Create(veaEvent.Id.ToString());
        
        //Act: execute the command
        var result = await _handler.HandleAsync(commandResult.Value!);
        
        //Assert
        Assert.False(result.IsErrorResult());
        Assert.Equal(VeaEventType.Private, veaEvent.VeaEventType);
    }
    
    private void Setup()
    {
        _veaEventBuilder = new VeaEventBuilder();
        _veaEventRepository = new FakeVeaEventRepositoryImpl();
        _handler = new MakeEventPrivateHandler(_veaEventRepository, new FakeUow());
    }
}