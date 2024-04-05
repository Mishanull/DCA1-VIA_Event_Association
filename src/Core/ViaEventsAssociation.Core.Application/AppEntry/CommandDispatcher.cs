using ViaEventsAssociation.Core.Application.AppEntry.Exceptions;
using ViaEventsAssociation.Core.Application.CommandHandler.Common.Base;
using VIAEventsAssociation.Core.Tools.OperationResult.OperationResult;

namespace ViaEventsAssociation.Core.Application.AppEntry;

internal class CommandDispatcher(IServiceProvider serviceProvider) : ICommandDispatcher
{
    public Task<Result> Dispatch(Command command)
    {
        Type handlerType = typeof(ICommandHandler<Command>);
        var service = serviceProvider.GetService(handlerType);
        if (service == null)
        {
            throw new ServiceNotFoundException("Service is not registered with the service provider");
        }

        ICommandHandler<Command> handler = (ICommandHandler<Command>)service;
        return handler.HandleAsync(command);
    }
}