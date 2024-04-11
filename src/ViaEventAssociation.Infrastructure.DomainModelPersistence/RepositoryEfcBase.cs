using Microsoft.EntityFrameworkCore;
using ViaEventAssociation.Core.Domain.Common.Base;
using ViaEventAssociation.Core.Domain.Contracts.Repositories;
using VIAEventsAssociation.Core.Tools.OperationResult.Helpers;
using VIAEventsAssociation.Core.Tools.OperationResult.OperationResult;

namespace ViaEventAssociation.Infrastructure.SqliteDmPersistence;

public class RepositoryEfcBase<TAgg, TYpeId>(DbContext context) : IVeaRepository<TAgg, TYpeId>
    where TAgg : AggregateRoot
    where TYpeId : TId
{
    public virtual async Task<Result<TAgg>> FindAsync(TYpeId id)
    {
        TAgg? root = await context.Set<TAgg>().FindAsync(id);
        // ToDo: Check if root is null?
        return ResultHelper.CreateSuccess(root!);
    }

    public virtual async Task<Result> AddAsync(TAgg entity)
    {
        await context.Set<TAgg>().AddAsync(entity);
        return ResultHelper.CreateSuccess();
    }

    public virtual async Task<Result> RemoveAsync(TYpeId id)
    {
        TAgg? root = await context.Set<TAgg>().FindAsync(id);
        context.Set<TAgg>().Remove(root!);
        return ResultHelper.CreateSuccess();
    }

    public virtual async Task<Result<TAgg>> UpdateAsync(TAgg entity)
    {
        throw new NotImplementedException();
    }
}