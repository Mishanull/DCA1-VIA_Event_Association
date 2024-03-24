using ViaEventAssociation.Core.Domain.Contracts.Repositories;
using ViaEventAssociation.Core.Domain.Contracts.UnitOfWork;
using ViaEventsAssociation.Core.Application.CommandDispatching.Commands.Event;
using ViaEventsAssociation.Core.Application.CommandDispatching.Common.Base;
using VIAEventsAssociation.Core.Tools.OperationResult.OperationResult;

namespace ViaEventsAssociation.Core.Application.CommandDispatching.Handlers.Event;

public class ActivateEventHandler : ICommandHandler<ActivateEventCommand>
{
    private readonly IVeaEventRepository _veaEventRepository;
    private readonly IUnitOfWork _unitOfWork;
    
    internal ActivateEventHandler(IVeaEventRepository veaEventRepository, IUnitOfWork unitOfWork)
    {
        _veaEventRepository = veaEventRepository;
        _unitOfWork = unitOfWork;
    }
    
    public async Task<Result> HandleAsync(ActivateEventCommand eventCommand)
    {
        var veaEventResult = await _veaEventRepository.FindAsync(eventCommand.VeaEventId);
        if (veaEventResult.IsErrorResult())
        {
            return veaEventResult;
        }
        
        var result = veaEventResult.Value!.Activate();
        if (result.IsErrorResult())
        {
            return result;
        }
        
        await _unitOfWork.SaveChangesAsync();
        return result;
    }
}