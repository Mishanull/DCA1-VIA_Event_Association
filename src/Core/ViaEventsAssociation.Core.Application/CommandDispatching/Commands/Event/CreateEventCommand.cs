using ViaEventAssociation.Core.Domain.Contracts;
using ViaEventAssociation.Core.Domain.Services;
using ViaEventsAssociation.Core.Application.CommandDispatching.Common.Base;
using VIAEventsAssociation.Core.Tools.OperationResult.OperationResult;

namespace ViaEventsAssociation.Core.Application.CommandDispatching.Commands.Event;

public class CreateEventCommand : Command
{
    internal ICurrentTime CurrentTime { get; }
    
    private CreateEventCommand(ICurrentTime currentTime)
    {
        CurrentTime = currentTime;
    }

    public static Result<CreateEventCommand> Create()
    {
        return new Result<CreateEventCommand>(new CreateEventCommand(new CurrentTime()));
    }
    
    protected override IEnumerable<object?> GetEqualityComponents()
    {
        throw new NotImplementedException();
    }
}