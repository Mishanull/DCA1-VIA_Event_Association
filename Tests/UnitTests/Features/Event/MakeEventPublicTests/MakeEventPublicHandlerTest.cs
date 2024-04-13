using UnitTests.FakeServices;
using UnitTests.FakeServices.Repositories;
using UnitTests.Utils;
using ViaEventAssociation.Core.Domain.EventAgg;
using ViaEventsAssociation.Core.Application.CommandHandler.Commands.Event;
using ViaEventsAssociation.Core.Application.CommandHandler.Common.Base;
using ViaEventsAssociation.Core.Application.CommandHandler.Handlers.Event;

namespace UnitTests.Features.Event.MakeEventPublicTests;

public class MakeEventPublicHandlerTest
{
    private VeaEventBuilder _veaEventBuilder;
    private IVeaEventRepository _veaEventRepository;
    private ICommandHandler<MakeEventPublicCommand> _handler;
    
    [Fact]
    public async void MakeEventPublic_Success()
    {
        //Arrange
        Setup();
        //create a private event
        var veaEvent = _veaEventBuilder.Init().WithEventType(VeaEventType.Private).Build();
        //add the event to the repository
        await _veaEventRepository.AddAsync(veaEvent);
        //create a command to make the event public
        var commandResult = MakeEventPublicCommand.Create(veaEvent.Id.ToString());
        
        //Act: execute the command
        var result = await _handler.HandleAsync(commandResult.Value!);
        
        //Assert
        Assert.False(result.IsErrorResult());
        Assert.Equal(VeaEventType.Public, veaEvent.VeaEventType);
    }
    
    private void Setup()
    {
        _veaEventBuilder = new VeaEventBuilder();
        _veaEventRepository = new FakeVeaEventRepositoryImpl();
        _handler = new MakeEventPublicHandler(_veaEventRepository, new FakeUow());
    }
}