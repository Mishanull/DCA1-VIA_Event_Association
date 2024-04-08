using ViaEventsAssociation.Core.Application.AppEntry.Exceptions;
using ViaEventsAssociation.Core.Application.CommandHandler.Common.Base;
using VIAEventsAssociation.Core.Tools.OperationResult.OperationResult;

namespace ViaEventsAssociation.Core.Application.AppEntry;

internal class CommandDispatcher(IServiceProvider serviceProvider) : ICommandDispatcher
{
    public Task<Result> Dispatch(Command command)
    {
        var handlerType = typeof(ICommandHandler<>).MakeGenericType(command.GetType());
        var service = serviceProvider.GetService(handlerType);
        if (service == null)
        {
            throw new ServiceNotFoundException("Service is not registered with the service provider");
        }
        
        var handleAsyncMethod = service.GetType().GetMethod("HandleAsync");
        if (handleAsyncMethod == null)
        {
            throw new InvalidOperationException("The handler does not implement HandleAsync method.");
        }
        
        return (Task<Result>)handleAsyncMethod.Invoke(service, [command])!;
    }
}