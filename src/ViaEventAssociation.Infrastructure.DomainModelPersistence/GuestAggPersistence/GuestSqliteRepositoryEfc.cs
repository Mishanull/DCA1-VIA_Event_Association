using Microsoft.EntityFrameworkCore;
using ViaEventAssociation.Core.Domain.GuestAgg;
using ViaEventAssociation.Core.Domain.GuestAgg.Guest;
using ViaEventAssociation.Core.Domain.GuestAgg.Request;
using ViaEventAssociation.Core.Domain.GuestAgg.RequestEntity;
using VIAEventsAssociation.Core.Tools.OperationResult.Error;
using VIAEventsAssociation.Core.Tools.OperationResult.Helpers;
using VIAEventsAssociation.Core.Tools.OperationResult.OperationResult;

namespace ViaEventAssociation.Infrastructure.SqliteDmPersistence.GuestAggPersistence;

public class GuestSqliteRepositoryEfc(WriteDbContext context)
    : RepositoryEfcBase<VeaGuest, GuestId>(context), IGuestRepository
{
    private readonly DbContext _context = context;

    public async Task<Result<Request>> FindRequestAsync(RequestId id)
    {
        var request = await _context.Set<Request>().FindAsync(id);
        if (request == null)
        {
            var errorResult = new Result<Request>(null);
            errorResult.CollectError(ErrorHelper.CreateVeaError("Request not found", ErrorType.ResourceNotFound));
            return errorResult;
        }

        return ResultHelper.CreateSuccess(request);
    }
}