using UnitTests.FakeServices;
using UnitTests.FakeServices.Repositories;
using UnitTests.Utils;
using ViaEventAssociation.Core.Domain.Contracts.Repositories;
using ViaEventAssociation.Core.Domain.EventAgg;
using ViaEventsAssociation.Core.Application.CommandDispatching.Commands.Event;
using ViaEventsAssociation.Core.Application.CommandDispatching.Common.Base;
using ViaEventsAssociation.Core.Application.CommandDispatching.Handlers.Event;
using VIAEventsAssociation.Core.Tools.OperationResult.Error;

namespace UnitTests.Features.Event.UpdateFromToTests;

public class UpdateFromToHandlerTest
{
    private VeaEventBuilder _veaEventBuilder;
    private IVeaEventRepository _veaEventRepository;
    private ICommandHandler<UpdateFromToCommand> _handler;
    
    private DateTime originalFrom = DateTime.Today.AddDays(1).AddHours(10);
    private DateTime originalTo = DateTime.Today.AddDays(1).AddHours(11);
    private DateTime newFrom = DateTime.Today.AddDays(2).AddHours(17);
    private DateTime newTo = DateTime.Today.AddDays(2).AddHours(20);
    
    [Fact]
    public async void UpdateFromTo_Success()
    {
        //Arrange
        Setup();
        
        //create an event
        var veaEvent = _veaEventBuilder.Init()
            .WithTitle("Test Event")
            .WithFromTo(originalFrom, originalTo)
            .Build();
        //add the event to the repository
        await _veaEventRepository.AddAsync(veaEvent);
        //create a command to update the from and to
        var commandResult = UpdateFromToCommand.Create(veaEvent.Id.ToString(), newFrom, newTo);
        
        //Act: execute the command
        var result = await _handler.HandleAsync(commandResult.Value!);
        
        //Assert
        Assert.False(result.IsErrorResult());
        Assert.Equal(newFrom, veaEvent.FromTo.Start);
        Assert.Equal(newTo, veaEvent.FromTo.End);
    }
    
    [Fact]
    public async void UpdateFromToOfCanceledEvent_Fail()
    {
        //Arrange
        Setup();
        //create an event
        var veaEvent = _veaEventBuilder.Init()
            .WithTitle("Test Event")
            .WithFromTo(originalFrom, originalTo)
            .WithStatus(VeaEventStatus.Cancelled)
            .Build();
        //add the event to the repository
        await _veaEventRepository.AddAsync(veaEvent);
        //create a command to update the from and to
        var commandResult = UpdateFromToCommand.Create(veaEvent.Id.ToString(), newFrom, newTo);
        //Act: execute the command
        var result = await _handler.HandleAsync(commandResult.Value!);
        
        Assert.True(result.IsErrorResult());
        Assert.Equal(ErrorType.ActionNotAllowed, result.Errors.First().Type);
        Assert.Equal(originalFrom, veaEvent.FromTo.Start);
        Assert.Equal(originalTo, veaEvent.FromTo.End);
    }
    
    [Fact]
    public async void UpdateFromToOfActiveEvent_Fail()
    {
        //Arrange
        Setup();
        //create an event
        var veaEvent = _veaEventBuilder.Init()
            .WithTitle("Test Event")
            .WithFromTo(originalFrom, originalTo)
            .WithStatus(VeaEventStatus.Active)
            .Build();
        //add the event to the repository
        await _veaEventRepository.AddAsync(veaEvent);
        //create a command to update the from and to
        var commandResult = UpdateFromToCommand.Create(veaEvent.Id.ToString(), newFrom, newTo);
        //Act: execute the command
        var result = await _handler.HandleAsync(commandResult.Value!);
        
        Assert.True(result.IsErrorResult());
        Assert.Equal(ErrorType.ActionNotAllowed, result.Errors.First().Type);
        Assert.Equal(originalFrom, veaEvent.FromTo.Start);
        Assert.Equal(originalTo, veaEvent.FromTo.End);
    }
    
    private void Setup()
    {
        _veaEventBuilder = new VeaEventBuilder();
        _veaEventRepository = new FakeVeaEventRepositoryImpl();
        _handler = new UpdateFromToHandler(_veaEventRepository, new FakeUow());
    }
    
}