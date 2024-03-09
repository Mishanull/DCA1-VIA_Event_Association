using ViaEventAssociation.Core.Domain.Common.Base;
using ViaEventAssociation.Core.Domain.EventAgg;
using VIAEventsAssociation.Core.Tools.OperationResult.OperationResult;

namespace ViaEventAssociation.Core.Domain.Contracts;

public interface IVeaRepository<T, TypeId> where TypeId : TId
{
    public Result<T> Save(T entity);
    public Result<T> Find(TypeId id);
    public Result Remove(TypeId id);
    public Result<T> Update(T entity);
}