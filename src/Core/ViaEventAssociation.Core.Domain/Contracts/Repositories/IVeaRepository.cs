using ViaEventAssociation.Core.Domain.Common.Base;
using VIAEventsAssociation.Core.Tools.OperationResult.OperationResult;

namespace ViaEventAssociation.Core.Domain.Contracts.Repositories;

public interface IVeaRepository<Tagg, TypeId>
    where Tagg : AggregateRoot
    where TypeId : TId
{
    public Task<Result<Tagg>> FindAsync(TypeId id);
    public Task<Result> AddAsync(Tagg entity); 
    public Task<Result> RemoveAsync(TypeId id);
}