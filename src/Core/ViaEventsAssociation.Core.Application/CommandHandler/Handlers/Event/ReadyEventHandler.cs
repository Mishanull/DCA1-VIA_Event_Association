using ViaEventAssociation.Core.Domain.Contracts.UnitOfWork;
using ViaEventAssociation.Core.Domain.EventAgg;
using ViaEventsAssociation.Core.Application.CommandHandler.Commands.Event;
using ViaEventsAssociation.Core.Application.CommandHandler.Common.Base;
using VIAEventsAssociation.Core.Tools.OperationResult.OperationResult;

namespace ViaEventsAssociation.Core.Application.CommandHandler.Handlers.Event;

public class ReadyEventHandler : ICommandHandler<ReadyEventCommand>
{
    private readonly IVeaEventRepository _veaEventRepository;
    private readonly IUnitOfWork _unitOfWork;
    
    internal ReadyEventHandler(IVeaEventRepository veaEventRepository, IUnitOfWork unitOfWork)
    {
        _veaEventRepository = veaEventRepository;
        _unitOfWork = unitOfWork;
    }
    
    public async Task<Result> HandleAsync(ReadyEventCommand eventCommand)
    {
        var veaEventResult = await _veaEventRepository.FindAsync(eventCommand.EventId);
        if (veaEventResult.IsErrorResult())
        {
            return veaEventResult;
        }
        
        var result = veaEventResult.Value!.Ready();
        if (result.IsErrorResult())
        {
            return result;
        }
        
        await _unitOfWork.SaveChangesAsync();
        return result;
    }
}