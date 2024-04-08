using ViaEventsAssociation.Core.Application.CommandHandler.Commands.Event;
using ViaEventsAssociation.Core.Application.CommandHandler.Common.Base;
using VIAEventsAssociation.Core.Tools.OperationResult.OperationResult;

namespace UnitTests.Common.Dispatcher;

public class CreateEventMockHandler : ICommandHandler<CreateEventCommand>
{
    public bool WasCalled { get; private set; } = false;
    public CreateEventCommand? LastHandledCommand { get; private set; } 
    public Task<Result> HandleAsync(CreateEventCommand command)
    {
        WasCalled = true;
        LastHandledCommand = command;
        return Task.FromResult(new Result());
    }
}