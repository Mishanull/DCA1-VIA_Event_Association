using ViaEventsAssociation.Core.Application.CommandHandler.Common.Base;
using VIAEventsAssociation.Core.Tools.OperationResult.OperationResult;

namespace ViaEventsAssociation.Core.Application.AppEntry;

public interface ICommandDispatcher
{
    public Task<Result> Dispatch (Command command);
}