using UnitTests.FakeServices;
using UnitTests.FakeServices.Repositories;
using UnitTests.Utils;
using ViaEventAssociation.Core.Domain.Contracts.Repositories;
using ViaEventAssociation.Core.Domain.EventAgg;
using ViaEventsAssociation.Core.Application.CommandDispatching.Commands.Event;
using ViaEventsAssociation.Core.Application.CommandDispatching.Common.Base;
using ViaEventsAssociation.Core.Application.CommandDispatching.Handlers.Event;
using VIAEventsAssociation.Core.Tools.OperationResult.Error;

namespace UnitTests.Features.Event.ReadyEventTests;

public class ReadyEventHandlerTest
{
    private VeaEventBuilder _veaEventBuilder;
    private IVeaEventRepository _veaEventRepository;
    private ICommandHandler<ReadyEventCommand> _handler;
    
    [Fact]
    public async void ReadyEvent_Success()
    {
        //Arrange
        Setup();
        //create a not ready event
        var veaEvent = _veaEventBuilder.Init()
            .WithTitle("Test Event")
            .WithDescription("Test Description")
            .WithFromTo(DateTime.Today.AddDays(1).AddHours(10), DateTime.Today.AddDays(1).AddHours(12))
            .WithEventType(VeaEventType.Public)
            .Build();
        //add the event to the repository
        await _veaEventRepository.AddAsync(veaEvent);
        //create a command to make the event ready
        var commandResult = ReadyEventCommand.Create(veaEvent.Id.ToString());
        
        //Act: execute the command
        var result = await _handler.HandleAsync(commandResult.Value!);
        
        //Assert
        Assert.False(result.IsErrorResult());
        Assert.Equal(VeaEventStatus.Ready, veaEvent.VeaEventStatus);
    }
    
    [Fact]
    public async void ReadyEvent_Fail() //event without description cannot be readied
    {
        //Arrange
        Setup();
        //create a not ready event
        var veaEvent = _veaEventBuilder.Init()
            .WithTitle("Test Event")
            // .WithDescription("Test Description")
            .WithFromTo(DateTime.Today.AddDays(1).AddHours(10), DateTime.Today.AddDays(1).AddHours(12))
            .WithEventType(VeaEventType.Public)
            .Build();
        //add the event to the repository
        await _veaEventRepository.AddAsync(veaEvent);
        //create a command to make the event ready
        var commandResult = ReadyEventCommand.Create(veaEvent.Id.ToString());
        
        //Act: execute the command
        var result = await _handler.HandleAsync(commandResult.Value!);
        
        //Assert
        Assert.True(result.IsErrorResult());
        Assert.Equal(result.Errors.First().Type, ErrorType.InvalidDescription);
    }
    
    private void Setup()
    {
        _veaEventBuilder = new VeaEventBuilder();
        _veaEventRepository = new FakeVeaEventRepositoryImpl();
        _handler = new ReadyEventHandler(_veaEventRepository, new FakeUow());
    }
    
}