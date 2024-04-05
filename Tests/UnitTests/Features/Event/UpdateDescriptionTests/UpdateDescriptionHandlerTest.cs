using UnitTests.FakeServices;
using UnitTests.FakeServices.Repositories;
using UnitTests.Utils;
using ViaEventAssociation.Core.Domain.Contracts.Repositories;
using ViaEventAssociation.Core.Domain.EventAgg;
using ViaEventsAssociation.Core.Application.CommandHandler.Commands.Event;
using ViaEventsAssociation.Core.Application.CommandHandler.Common.Base;
using ViaEventsAssociation.Core.Application.CommandHandler.Handlers.Event;
using VIAEventsAssociation.Core.Tools.OperationResult.Error;

namespace UnitTests.Features.Event.UpdateDescriptionTests;

public class UpdateDescriptionHandlerTest
{
    private VeaEventBuilder _veaEventBuilder;
    private IVeaEventRepository _veaEventRepository;
    private ICommandHandler<UpdateDescriptionCommand> _handler;
    
    [Fact]
    public async void UpdateDescription_Success()
    {
        //Arrange
        Setup();
        const string originalDescription = "Original valid Description";
        const string newDescription = "New valid Description";
        //create an event
        var veaEvent = _veaEventBuilder.Init()
            .WithTitle("Test Event")
            .WithDescription(originalDescription)
            .Build();
        //add the event to the repository
        await _veaEventRepository.AddAsync(veaEvent);
        //create a command to update the description
        var commandResult = UpdateDescriptionCommand.Create(veaEvent.Id.ToString(), newDescription);
        
        //Act: execute the command
        var result = await _handler.HandleAsync(commandResult.Value!);
        
        //Assert
        Assert.False(result.IsErrorResult());
        Assert.Equal(newDescription, veaEvent.Description.Value);
    }
    
    [Fact]
    public async void UpdateDescriptionOfCanceledEvent_Fail()
    {
        //Arrange
        Setup();
        const string originalDescription = "Test Description";
        const string newDescription = "New Description";
        //create an event
        var veaEvent = _veaEventBuilder.Init()
            .WithTitle("Test Event")
            .WithDescription(originalDescription)
            .WithStatus(VeaEventStatus.Cancelled)
            .Build();
        //add the event to the repository
        await _veaEventRepository.AddAsync(veaEvent);
        //create a command to update the description
        var commandResult = UpdateDescriptionCommand.Create(veaEvent.Id.ToString(), newDescription);
        //Act: execute the command
        var result = await _handler.HandleAsync(commandResult.Value!);
        
        //Assert
        Assert.True(result.IsErrorResult());
        Assert.Equal(ErrorType.ActionNotAllowed, result.Errors.First().Type);
        Assert.Equal(originalDescription, veaEvent.Description.Value);
    }

    [Fact]
    public async void UpdateDescriptionOfActiveEvent_Fail()
    {
        //Arrange
        Setup();
        const string originalDescription = "Test Description";
        const string newDescription = "New Description";
        //create an event
        var veaEvent = _veaEventBuilder.Init()
            .WithTitle("Test Event")
            .WithDescription(originalDescription)
            .WithStatus(VeaEventStatus.Active)
            .Build();
        //add the event to the repository
        await _veaEventRepository.AddAsync(veaEvent);
        //create a command to update the description
        var commandResult = UpdateDescriptionCommand.Create(veaEvent.Id.ToString(), newDescription);
        //Act: execute the command
        var result = await _handler.HandleAsync(commandResult.Value!);
        
        //Assert
        Assert.True(result.IsErrorResult());
        Assert.Equal(ErrorType.ActionNotAllowed, result.Errors.First().Type);
        Assert.Equal(originalDescription, veaEvent.Description.Value);
    }
    
    private void Setup()
    {
        _veaEventBuilder = new VeaEventBuilder();
        _veaEventRepository = new FakeVeaEventRepositoryImpl();
        _handler = new UpdateDescriptionHandler(_veaEventRepository, new FakeUow());
    }
    
}