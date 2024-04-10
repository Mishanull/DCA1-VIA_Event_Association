using ViaEventAssociation.Core.Domain.Contracts.Repositories;
using ViaEventAssociation.Core.Domain.LocationAgg;

namespace ViaEventAssociation.Infrastructure.SqliteDmPersistence.LocationPersistence;

public class LocationSqliteRepositoryEfc(WriteDbContext context) : RepositoryEfcBase<Location, LocationId>, ILocationRepository;