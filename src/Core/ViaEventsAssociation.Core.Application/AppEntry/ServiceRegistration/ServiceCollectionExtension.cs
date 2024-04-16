using ViaEventsAssociation.Core.Application.CommandHandler.Common.Base;

namespace ViaEventsAssociation.Core.Application.AppEntry.ServiceRegistration;

using Microsoft.Extensions.DependencyInjection;
using System.Linq;
using System.Reflection;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddCommandHandlers(this IServiceCollection services, Assembly assembly)
    {
        var commandHandlers = assembly.GetTypes()
            .Where(t => t.GetInterfaces().Any(i => i.IsGenericType && i.GetGenericTypeDefinition() == typeof(ICommandHandler<>)))
            .Where(t => t is { IsAbstract: false, IsPublic: true });

        foreach (var handlerType in commandHandlers)
        {
            var interfaceType = handlerType.GetInterfaces()
                .First(i => i.IsGenericType && i.GetGenericTypeDefinition() == typeof(ICommandHandler<>));
            services.AddScoped(interfaceType, handlerType);
        }

        return services;
    }
}