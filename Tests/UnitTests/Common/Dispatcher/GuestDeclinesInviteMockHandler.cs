using ViaEventsAssociation.Core.Application.CommandHandler.Commands.Guest;
using ViaEventsAssociation.Core.Application.CommandHandler.Common.Base;
using VIAEventsAssociation.Core.Tools.OperationResult.OperationResult;

namespace UnitTests.Common.Dispatcher;

public class GuestDeclinesInviteMockHandler : ICommandHandler<GuestDeclinesInviteCommand>
{
    public bool WasCalled { get; private set; } = false;
    public GuestDeclinesInviteCommand? LastHandledCommand { get; private set; } 
    public Task<Result> HandleAsync(GuestDeclinesInviteCommand command)
    {
        WasCalled = true;
        LastHandledCommand = command;
        return Task.FromResult(new Result());
    }
}