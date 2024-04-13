using UnitTests.FakeServices;
using UnitTests.FakeServices.Repositories;
using UnitTests.Utils;
using ViaEventAssociation.Core.Domain.EventAgg;
using ViaEventsAssociation.Core.Application.CommandHandler.Commands.Event;
using ViaEventsAssociation.Core.Application.CommandHandler.Common.Base;
using ViaEventsAssociation.Core.Application.CommandHandler.Handlers.Event;
using VIAEventsAssociation.Core.Tools.OperationResult.Error;

namespace UnitTests.Features.Event.UpdateMaxGuestsTests;

public class UpdateMaxGuestsHandlerTest
{
    private VeaEventBuilder _veaEventBuilder;
    private IVeaEventRepository _veaEventRepository;
    private ICommandHandler<UpdateMaxGuestsCommand> _handler;
    
    [Fact]
    public async void UpdateMaxGuests_Success()
    {
        //Arrange
        Setup();
        //create an event
        var veaEvent = _veaEventBuilder.Init()
            .WithTitle("Test Event")
            .WithDescription("Test Description")
            .WithFromTo(DateTime.Today.AddDays(1).AddHours(10), DateTime.Today.AddDays(1).AddHours(12))
            .WithEventType(VeaEventType.Public)
            .Build();
        //add the event to the repository
        await _veaEventRepository.AddAsync(veaEvent);
        //create a command to update the max guests
        var commandResult = UpdateMaxGuestsCommand.Create(veaEvent.Id.ToString(), 10);
        
        //Act: execute the command
        var result = await _handler.HandleAsync(commandResult.Value!);
        
        //Assert
        Assert.False(result.IsErrorResult());
        Assert.Equal(10, veaEvent.MaxGuests.Value);
    }
    
    [Fact]
    public async void UpdateMaxGuestsOfCanceledEvent_Fail()
    {
        //Arrange
        Setup();
        const int originalMaxGuests = 10;
        const int newMaxGuests = 11;
        //create an event
        var veaEvent = _veaEventBuilder.Init()
            .WithTitle("Test Event")
            .WithDescription("Test Description")
            .WithFromTo(DateTime.Today.AddDays(1).AddHours(10), DateTime.Today.AddDays(1).AddHours(12))
            .WithEventType(VeaEventType.Public)
            .WithStatus(VeaEventStatus.Cancelled)
            .WithMaxGuests(originalMaxGuests)
            .Build();
        //add the event to the repository
        await _veaEventRepository.AddAsync(veaEvent);
        //create a command to update the max guests
        var commandResult = UpdateMaxGuestsCommand.Create(veaEvent.Id.ToString(), newMaxGuests);
        
        //Act: execute the command
        var result = await _handler.HandleAsync(commandResult.Value!);
        
        //Assert
        Assert.True(result.IsErrorResult());
        Assert.Equal(originalMaxGuests, veaEvent.MaxGuests.Value);
        Assert.Equal(ErrorType.InvalidMaxGuests, result.Errors.First().Type);
    }
    
    
    [Fact]
    public async void IncreaseMaxGuestsOfActiveEvent_Success()
    {
        //Arrange
        Setup();
        const int originalMaxGuests = 10;
        const int newMaxGuests = 11;
        //create an event
        var veaEvent = _veaEventBuilder.Init()
            .WithTitle("Test Event")
            .WithDescription("Test Description")
            .WithFromTo(DateTime.Today.AddDays(1).AddHours(10), DateTime.Today.AddDays(1).AddHours(12))
            .WithEventType(VeaEventType.Public)
            .WithStatus(VeaEventStatus.Active)
            .WithMaxGuests(originalMaxGuests)
            .Build();
        //add the event to the repository
        await _veaEventRepository.AddAsync(veaEvent);
        //create a command to update the max guests
        var commandResult = UpdateMaxGuestsCommand.Create(veaEvent.Id.ToString(), newMaxGuests);
        
        //Act: execute the command
        var result = await _handler.HandleAsync(commandResult.Value!);
        
        //Assert
        Assert.False(result.IsErrorResult());
        Assert.Equal(newMaxGuests, veaEvent.MaxGuests.Value);
    }
    
    [Fact]
    public async void ReduceMaxGuestsOfActiveEvent_Fail()
    {
        //Arrange
        Setup();
        const int originalMaxGuests = 10;
        const int newMaxGuests = 9;
        //create an event
        var veaEvent = _veaEventBuilder.Init()
            .WithTitle("Test Event")
            .WithDescription("Test Description")
            .WithFromTo(DateTime.Today.AddDays(1).AddHours(10), DateTime.Today.AddDays(1).AddHours(12))
            .WithEventType(VeaEventType.Public)
            .WithStatus(VeaEventStatus.Active)
            .WithMaxGuests(originalMaxGuests)
            .Build();
        //add the event to the repository
        await _veaEventRepository.AddAsync(veaEvent);
        //create a command to update the max guests
        var commandResult = UpdateMaxGuestsCommand.Create(veaEvent.Id.ToString(), newMaxGuests);
        
        //Act: execute the command
        var result = await _handler.HandleAsync(commandResult.Value!);
        
        //Assert
        Assert.True(result.IsErrorResult());
        Assert.Equal(originalMaxGuests, veaEvent.MaxGuests.Value);
        Assert.Equal(ErrorType.InvalidMaxGuests, result.Errors.First().Type);
    }
    
    private void Setup()
    {
        _veaEventBuilder = new VeaEventBuilder();
        _veaEventRepository = new FakeVeaEventRepositoryImpl();
        _handler = new UpdateMaxGuestsHandler(_veaEventRepository, new FakeUow());
    }
}