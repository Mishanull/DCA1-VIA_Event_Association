using Microsoft.EntityFrameworkCore;
using ViaEventAssociation.Core.Domain.LocationAgg;

namespace ViaEventAssociation.Infrastructure.SqliteDmPersistence.LocationAggPersistence;

public class LocationSqliteRepositoryEfc(WriteDbContext context) : RepositoryEfcBase<Location, LocationId>(context), ILocationRepository;