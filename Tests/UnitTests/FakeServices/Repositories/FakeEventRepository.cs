using UnitTests.Utils;
using ViaEventAssociation.Core.Domain.Contracts.Repositories;
using ViaEventAssociation.Core.Domain.EventAgg;
using VIAEventsAssociation.Core.Tools.OperationResult.OperationResult;

namespace UnitTests.FakeServices.Repositories;

public class FakeEventRepository : IVeaEventRepository
{

    public Result<VeaEvent> Find(VeaEventId id)
    {
        var builder = new VeaEventBuilder();
        builder.Init();
        return new Result<VeaEvent>(builder.WithStatus(VeaEventStatus.Active)
            .WithTime(new DateTime(2024,06,23))
            .WithEventType(VeaEventType.Public)
            .WithFromTo(new DateTime(2024, 07, 01), new DateTime(2024, 07,03))
            .Build());

    }
    
    public async Task<Result<VeaEvent>> FindAsync(VeaEventId id)
    {
        var builder = new VeaEventBuilder();
        builder.Init();
        return new Result<VeaEvent>(builder.WithStatus(VeaEventStatus.Active).WithTime(new DateTime(2024,06,23)).Build());
    }
    
    public async Task<Result> AddAsync(VeaEvent entity)
    {
        return new Result();
    }
    
    public Result Remove(VeaEventId id)
    {
        throw new NotImplementedException();
    }
    public Task<Result> RemoveAsync(VeaEventId id)
    {
        throw new NotImplementedException();
    }
    public Result<VeaEvent> Update(VeaEvent entity)
    {
        throw new NotImplementedException();
    }
    public async Task<Result<VeaEvent>> UpdateAsync(VeaEvent entity)
    {
        return new Result<VeaEvent>(entity);
    }
}