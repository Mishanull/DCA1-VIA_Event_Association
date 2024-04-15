using Microsoft.EntityFrameworkCore;
using ViaEventAssociation.Core.Domain.CreatorAgg;
using ViaEventAssociation.Core.Domain.CreatorAgg.InviteEntity;
using VIAEventsAssociation.Core.Tools.OperationResult.Error;
using VIAEventsAssociation.Core.Tools.OperationResult.Helpers;
using VIAEventsAssociation.Core.Tools.OperationResult.OperationResult;

namespace ViaEventAssociation.Infrastructure.SqliteDmPersistence.CreatorAggPersistence;

public class CreatorSqliteRepositoryEfc(DbContext context)
    : RepositoryEfcBase<Creator, CreatorId>(context), ICreatorRepository
{
    public async Task<Result<Invite>> FindInviteAsync(InviteId id)
    {
        var invite = await context.Set<Invite>().FindAsync(id);
        if (invite == null)
        {
            var errorResult = new Result<Invite>(null);
            errorResult.CollectError(ErrorHelper.CreateVeaError("Invite not found", ErrorType.ResourceNotFound));
            return errorResult;
        }

        return ResultHelper.CreateSuccess(invite);
    }
}