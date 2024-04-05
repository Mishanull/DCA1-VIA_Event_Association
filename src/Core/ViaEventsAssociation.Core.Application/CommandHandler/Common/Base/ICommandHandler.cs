using VIAEventsAssociation.Core.Tools.OperationResult.OperationResult;

namespace ViaEventsAssociation.Core.Application.CommandHandler.Common.Base;

public interface ICommandHandler<in T> where T : Command  
{
    public Task<Result> HandleAsync(T command);
}