using UnitTests.FakeServices;
using UnitTests.FakeServices.Repositories;
using UnitTests.Utils;
using ViaEventAssociation.Core.Domain.Contracts.Repositories;
using ViaEventAssociation.Core.Domain.EventAgg;
using ViaEventsAssociation.Core.Application.CommandDispatching.Commands.Event;
using ViaEventsAssociation.Core.Application.CommandDispatching.Common.Base;
using ViaEventsAssociation.Core.Application.CommandDispatching.Handlers.Event;
using VIAEventsAssociation.Core.Tools.OperationResult.Error;

namespace UnitTests.Features.Event.UpdateTitleTests;

public class UpdateTitleHandlerTest
{
    private VeaEventBuilder _veaEventBuilder;
    private IVeaEventRepository _veaEventRepository;
    private ICommandHandler<UpdateTitleCommand> _handler;
    
    private string originalTitle = "Original valid Title";
    private string newTitle = "New valid Title";
    
    [Fact]
    public async void UpdateTitle_Success()
    {
        //Arrange
        Setup();
        //create an event
        var veaEvent = _veaEventBuilder.Init()
            .WithTitle(originalTitle)
            .Build();
        //add the event to the repository
        await _veaEventRepository.AddAsync(veaEvent);
        //create a command to update the title
        var commandResult = UpdateTitleCommand.Create(veaEvent.Id.ToString(), newTitle);
        
        //Act: execute the command
        var result = await _handler.HandleAsync(commandResult.Value!);
        
        //Assert
        Assert.False(result.IsErrorResult());
        Assert.Equal(newTitle, veaEvent.Title.Value);
    }
    
    [Fact]
    public async void UpdateTitleOfCanceledEvent_Fail()
    {
        //Arrange
        Setup();
        //create an event
        var veaEvent = _veaEventBuilder.Init()
            .WithTitle(originalTitle)
            .WithStatus(VeaEventStatus.Cancelled)
            .Build();
        //add the event to the repository
        await _veaEventRepository.AddAsync(veaEvent);
        //create a command to update the title
        var commandResult = UpdateTitleCommand.Create(veaEvent.Id.ToString(), newTitle);
        //Act: execute the command
        var result = await _handler.HandleAsync(commandResult.Value!);
        
        //Assert
        Assert.True(result.IsErrorResult());
        Assert.Equal(ErrorType.ActionNotAllowed, result.Errors.First().Type);
        Assert.Equal(originalTitle, veaEvent.Title.Value);
    }
    
    private void Setup()
    {
        _veaEventBuilder = new VeaEventBuilder();
        _veaEventRepository = new FakeVeaEventRepositoryImpl();
        _handler = new UpdateTitleHandler(_veaEventRepository, new FakeUow());
    }
    
}