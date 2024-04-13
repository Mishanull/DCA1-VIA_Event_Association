using Microsoft.EntityFrameworkCore;
using ViaEventAssociation.Core.Domain.LocationAgg;

namespace ViaEventAssociation.Infrastructure.SqliteDmPersistence.LocationAggPersistence;

public class LocationSqliteRepositoryEfc(DbContext context) : RepositoryEfcBase<Location, LocationId>(context), ILocationRepository;