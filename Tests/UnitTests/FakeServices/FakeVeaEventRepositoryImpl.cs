using ViaEventAssociation.Core.Domain.Contracts.Repositories;
using ViaEventAssociation.Core.Domain.EventAgg;
using VIAEventsAssociation.Core.Tools.OperationResult.Error;
using VIAEventsAssociation.Core.Tools.OperationResult.Helpers;
using VIAEventsAssociation.Core.Tools.OperationResult.OperationResult;

namespace UnitTests.FakeServices;

public class FakeVeaEventRepositoryImpl : IVeaRepository<VeaEvent, VeaEventId>
{
    public List<VeaEvent> VeaEvents { get; set; }
    
    public Result<VeaEvent> Find(VeaEventId id)
    {
        var veaEvent = VeaEvents.FirstOrDefault(ve => ve.Id == id);
        var result = new Result<VeaEvent>(veaEvent);
        if (veaEvent == null)
        {
            result.CollectError(ErrorHelper.CreateVeaError("VeaEvent not found", ErrorType.ResourceNotFound));
        }
        return result;
    }

    public async Task<Result<VeaEvent>> FindAsync(VeaEventId id)
    {
        var veaEvent = VeaEvents.FirstOrDefault(ve => ve.Id == id);
        var result = new Result<VeaEvent>(veaEvent);
        if (veaEvent == null)
        {
            result.CollectError(ErrorHelper.CreateVeaError("VeaEvent not found", ErrorType.ResourceNotFound));
        }
        return result;
    }

    public async Task<Result> AddAsync(VeaEvent entity)
    {
        VeaEvents.Add(entity);
        return ResultHelper.CreateSuccess();
    }

    public Result Remove(VeaEventId id)
    {
        var veaEvent = VeaEvents.FirstOrDefault(ve => ve.Id == id);
        if (veaEvent != null)
        {
            VeaEvents.Remove(veaEvent);
            return ResultHelper.CreateSuccess();
        }
        return ResultHelper.CreateErrorResultWithSingleError(ErrorType.ResourceNotFound, new ErrorMessage("VeaEvent not found"));
    }

    public async Task<Result> RemoveAsync(VeaEventId id)
    {
        var veaEvent = VeaEvents.FirstOrDefault(ve => ve.Id == id);
        if (veaEvent != null)
        {
            VeaEvents.Remove(veaEvent);
            return ResultHelper.CreateSuccess();
        }
        return ResultHelper.CreateErrorResultWithSingleError(ErrorType.ResourceNotFound, new ErrorMessage("VeaEvent not found"));
    }

    public Result<VeaEvent> Update(VeaEvent entity)
    {
        throw new NotImplementedException();
    }

    public Result<VeaEvent> UpdateAsync(VeaEvent entity)
    {
        throw new NotImplementedException();
    }
}