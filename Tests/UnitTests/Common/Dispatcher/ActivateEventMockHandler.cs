using ViaEventsAssociation.Core.Application.CommandHandler.Commands.Event;
using ViaEventsAssociation.Core.Application.CommandHandler.Common.Base;
using VIAEventsAssociation.Core.Tools.OperationResult.OperationResult;

namespace UnitTests.Common.Dispatcher;

public class ActivateEventMockHandler : ICommandHandler<ActivateEventCommand>
{
    public bool WasCalled { get; private set; } = false;
    public ActivateEventCommand? LastHandledCommand { get; private set; }
    public Task<Result> HandleAsync(ActivateEventCommand command)
    {
        WasCalled = true;
        LastHandledCommand = command;
        return Task.FromResult(new Result());
    }
}