using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using ViaEventAssociation.Core.Domain.Common;
using ViaEventAssociation.Core.Domain.Contracts.UnitOfWork;
using ViaEventAssociation.Core.Domain.CreatorAgg;
using ViaEventAssociation.Core.Domain.EventAgg;
using ViaEventAssociation.Core.Domain.GuestAgg;
using ViaEventAssociation.Core.Domain.LocationAgg;
using ViaEventAssociation.Infrastructure.SqliteDmPersistence.CreatorAggPersistence;
using ViaEventAssociation.Infrastructure.SqliteDmPersistence.EventAggPersistence;
using ViaEventAssociation.Infrastructure.SqliteDmPersistence.GuestAggPersistence;
using ViaEventAssociation.Infrastructure.SqliteDmPersistence.LocationAggPersistence;
using ViaEventAssociation.Infrastructure.SqliteDmPersistence.UnitOfWork;

namespace ViaEventAssociation.Infrastructure.SqliteDmPersistence.ServiceRegistration;

public static class ServiceCollectionExtensions
{
    public static void RegisterRepositories(this IServiceCollection services)
    {
        services.AddScoped<IVeaEventRepository, EventSqliteRepositoryEfc>();
        services.AddScoped<IGuestRepository, GuestSqliteRepositoryEfc>();
        services.AddScoped<ILocationRepository, LocationSqliteRepositoryEfc>();
        services.AddScoped<ICreatorRepository, CreatorSqliteRepositoryEfc>();
        services.AddScoped<IUnitOfWork, SqliteUnitOfWork>();
    }
}