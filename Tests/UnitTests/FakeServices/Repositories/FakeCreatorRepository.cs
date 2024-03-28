using ViaEventAssociation.Core.Domain.Common.ValueObjects;
using ViaEventAssociation.Core.Domain.Contracts.Repositories;
using ViaEventAssociation.Core.Domain.CreatorAgg;
using VIAEventsAssociation.Core.Tools.OperationResult.OperationResult;

namespace UnitTests.FakeServices.Repositories;

public class FakeCreatorRepository : ICreatorRepository
{

    public Result<Creator> Find(CreatorId id)
    {
        
        return new Result<Creator>(Creator.Create(Email.Create("example@via.com", new FakeEmailCheck()).Value!).Value!);

    }
    
    public async Task<Result<Creator>> FindAsync(CreatorId id)
    {
        return new Result<Creator>(Creator.Create(Email.Create("example@via.com", new FakeEmailCheck()).Value!).Value!);
    }
    
    public async Task<Result> AddAsync(Creator entity)
    {
        return new Result();
    }
    
    public  Result Remove(CreatorId id)
    {
        throw new NotImplementedException();
    }
    
    public async Task<Result> RemoveAsync(CreatorId id)
    {
        throw new NotImplementedException();
    }
    
    public Result<Creator> Update(Creator entity)
    {
        throw new NotImplementedException();
    }
    
    public async Task<Result<Creator>> UpdateAsync(Creator entity)
    {
        return new Result<Creator>(entity);
    }
}