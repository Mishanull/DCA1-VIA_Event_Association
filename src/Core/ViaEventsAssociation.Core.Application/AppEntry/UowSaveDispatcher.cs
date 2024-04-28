using ViaEventAssociation.Core.Domain.Contracts.UnitOfWork;
using ViaEventsAssociation.Core.Application.CommandHandler.Common.Base;
using VIAEventsAssociation.Core.Tools.OperationResult.OperationResult;

namespace ViaEventsAssociation.Core.Application.AppEntry;

public class UowSaveDispatcher(ICommandDispatcher next, IUnitOfWork unitOfWork) : ICommandDispatcher
{
    public async Task<Result> Dispatch(Command command)
    {
        var dispatchResult = await next.Dispatch(command);
        if (dispatchResult.IsErrorResult())
        {
            return dispatchResult;
        }

        await unitOfWork.SaveChangesAsync();
        return new Result();
    }
}