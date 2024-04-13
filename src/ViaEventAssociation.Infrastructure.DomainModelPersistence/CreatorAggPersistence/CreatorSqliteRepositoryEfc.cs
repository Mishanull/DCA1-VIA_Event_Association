using Microsoft.EntityFrameworkCore;
using ViaEventAssociation.Core.Domain.CreatorAgg;
using ViaEventAssociation.Core.Domain.CreatorAgg.InviteEntity;
using VIAEventsAssociation.Core.Tools.OperationResult.OperationResult;

namespace ViaEventAssociation.Infrastructure.SqliteDmPersistence.CreatorAggPersistence;

public class CreatorSqliteRepositoryEfc(DbContext context) : RepositoryEfcBase<Creator, CreatorId>(context), ICreatorRepository
{
    //ToDo: not sure about this implementation
    public Task<Result<Invite>> FindInviteAsync(InviteId id)
    {
        throw new NotImplementedException();
    }
}