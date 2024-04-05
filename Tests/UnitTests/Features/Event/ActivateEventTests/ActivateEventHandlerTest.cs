using UnitTests.FakeServices;
using UnitTests.FakeServices.Repositories;
using UnitTests.Utils;
using ViaEventAssociation.Core.Domain.Contracts.Repositories;
using ViaEventAssociation.Core.Domain.Contracts.UnitOfWork;
using ViaEventAssociation.Core.Domain.EventAgg;
using ViaEventsAssociation.Core.Application.CommandHandler.Commands.Event;
using ViaEventsAssociation.Core.Application.CommandHandler.Common.Base;
using ViaEventsAssociation.Core.Application.CommandHandler.Handlers.Event;

namespace UnitTests.Features.Event.ActivateEventTests;

public class ActivateEventHandlerTest
{
    private VeaEventBuilder _veaEventBuilder;
    private IVeaEventRepository _veaEventRepository;
    private ICommandHandler<ActivateEventCommand> _handler;
    private IUnitOfWork _uow;
    
    [Fact]
    public async void ActivateEvent_Success()
    {
        //Arrange
        Setup();
        
        //create default event
        var veaEvent = _veaEventBuilder.Init()
            .WithTitle("Test Event")
            .WithDescription("Test Description")
            //from 10:00 tmrw until 11:00 tmrw *Today returns 00:00
            .WithFromTo( DateTime.Today.AddDays(1).AddHours(10), DateTime.Today.AddDays(1).AddHours(11))
            .Build(); //event cannot be activated unless these fields are set!
        await _veaEventRepository.AddAsync(veaEvent);
        // _uow.SaveChangesAsync();
        
        //create valid command
        var eventGuid = veaEvent.Id.ToString();
        var commandResult = ActivateEventCommand.Create(eventGuid!);
        
        //Act
        var result = await _handler.HandleAsync(commandResult.Value!);
        
        //Assert
        Assert.False(result.IsErrorResult());
        Assert.Equal( VeaEventStatus.Active, veaEvent.VeaEventStatus);
    }

    private void Setup()
    {
        _veaEventBuilder = new VeaEventBuilder();
        _veaEventRepository = new FakeVeaEventRepositoryImpl();
        _uow = new FakeUow();
        _handler = new ActivateEventHandler(_veaEventRepository, _uow);
    }
}