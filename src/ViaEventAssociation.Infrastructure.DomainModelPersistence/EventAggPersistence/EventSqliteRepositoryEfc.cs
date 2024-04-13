using Microsoft.EntityFrameworkCore;
using ViaEventAssociation.Core.Domain.EventAgg;

namespace ViaEventAssociation.Infrastructure.SqliteDmPersistence.EventAggPersistence;

public class EventSqliteRepositoryEfc(DbContext context) : RepositoryEfcBase<VeaEvent, VeaEventId>(context), IVeaEventRepository;