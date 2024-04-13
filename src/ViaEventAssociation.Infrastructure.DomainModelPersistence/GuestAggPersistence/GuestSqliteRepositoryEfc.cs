using Microsoft.EntityFrameworkCore;
using ViaEventAssociation.Core.Domain.GuestAgg;
using ViaEventAssociation.Core.Domain.GuestAgg.Guest;
using ViaEventAssociation.Core.Domain.GuestAgg.Request;
using ViaEventAssociation.Core.Domain.GuestAgg.RequestEntity;
using VIAEventsAssociation.Core.Tools.OperationResult.OperationResult;

namespace ViaEventAssociation.Infrastructure.SqliteDmPersistence.GuestAggPersistence;

public class GuestSqliteRepositoryEfc(DbContext context) : RepositoryEfcBase<VeaGuest, GuestId>(context), IGuestRepository
{
    public Task<Result<Request>> FindRequestAsync(RequestId id)
    {
        throw new NotImplementedException();
    }
}