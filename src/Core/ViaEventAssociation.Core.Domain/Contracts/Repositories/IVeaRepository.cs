using ViaEventAssociation.Core.Domain.Common.Base;
using VIAEventsAssociation.Core.Tools.OperationResult.OperationResult;

namespace ViaEventAssociation.Core.Domain.Contracts.Repositories;

public interface IVeaRepository<T, TypeId> where TypeId : TId
{
    public Result<T> Find(TypeId id);
    public Task<Result<T>> FindAsync(TypeId id);
    public Task<Result> AddAsync(T entity); 
    public Result Remove(TypeId id);
    public Task<Result> RemoveAsync(TypeId id);
    public Result<T> Update(T entity);
    public Result<T> UpdateAsync(T entity);
}