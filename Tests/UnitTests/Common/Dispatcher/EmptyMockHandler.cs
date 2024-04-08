using ViaEventsAssociation.Core.Application.CommandHandler.Commands.Guest;
using ViaEventsAssociation.Core.Application.CommandHandler.Common.Base;
using VIAEventsAssociation.Core.Tools.OperationResult.OperationResult;

namespace UnitTests.Common.Dispatcher;

public class EmptyMockHandler : ICommandHandler<GuestAcceptsInviteCommand>
{
    public Task<Result> HandleAsync(GuestAcceptsInviteCommand command)
    {
        throw new NotImplementedException();
    }
}