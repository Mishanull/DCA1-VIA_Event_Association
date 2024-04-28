using System.Reflection;
using Microsoft.Extensions.DependencyInjection;
using ViaEventAssociation.Core.QueryContracts.Contract;
using ViaEventAssociation.Core.QueryContracts.QueryDispatching;

namespace ViaEventAssociation.Core.QueryContracts.ServiceRegistration;

public static class QueryHandlersRegistration
{
    public static void AddQueryHandlers(this IServiceCollection services, Assembly assembly)
    {
        var queryHandlers = assembly.GetTypes()
            .Where(t => t.GetInterfaces().Any(i => i.IsGenericType && i.GetGenericTypeDefinition() == typeof(IQueryHandler<,>)))
            .Where(t => t is { IsAbstract: false, IsPublic: true });

        foreach (var handlerType in queryHandlers)
        {
            var interfaceType = handlerType.GetInterfaces()
                .First(i => i.IsGenericType && i.GetGenericTypeDefinition() == typeof(IQueryHandler<,>));
            services.AddScoped(interfaceType, handlerType);
        }
    }
    
    public static void RegisterQueryDispatcher(this IServiceCollection services)
    {
        services.AddScoped<IQueryDispatcher, QueryDispatcher>();
    }
}