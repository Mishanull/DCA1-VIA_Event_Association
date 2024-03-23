using ViaEventAssociation.Core.Domain.Contracts.Repositories;
using ViaEventAssociation.Core.Domain.EventAgg;
using ViaEventAssociation.Core.Domain.GuestAgg.Guest;
using ViaEventAssociation.Core.Domain.GuestAgg.Request;
using ViaEventAssociation.Core.Domain.GuestAgg.RequestEntity;
using VIAEventsAssociation.Core.Tools.OperationResult.OperationResult;

namespace UnitTests.FakeServices.Repositories;

public class FakeRequestRepository : IRequestRepository
{

    public Result<Request> Find(RequestId id)
    {
        return new Result<Request>(Request.Create("valid reason", new VeaEventId(), new GuestId()).Value);
    }
    
    public async Task<Result<Request>> FindAsync(RequestId id)
    {
        return new Result<Request>(Request.Create("valid reason", new VeaEventId(), new GuestId()).Value);
    }
    
    public async Task<Result> AddAsync(Request entity)
    {
        return new Result();
    }
    
    public Result Remove(RequestId id)
    {
        throw new NotImplementedException();
    }
    
    public Task<Result> RemoveAsync(RequestId id)
    {
        throw new NotImplementedException();
    }
    
    public Result<Request> Update(Request entity)
    {
        throw new NotImplementedException();
    }
    
    public async Task<Result<Request>> UpdateAsync(Request entity)
    {
        return new Result<Request>(entity);
    }
}