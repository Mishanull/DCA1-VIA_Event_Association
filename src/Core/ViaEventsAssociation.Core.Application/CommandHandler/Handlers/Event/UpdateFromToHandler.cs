using ViaEventAssociation.Core.Domain.Contracts.UnitOfWork;
using ViaEventAssociation.Core.Domain.EventAgg;
using ViaEventsAssociation.Core.Application.CommandHandler.Commands.Event;
using ViaEventsAssociation.Core.Application.CommandHandler.Common.Base;
using VIAEventsAssociation.Core.Tools.OperationResult.OperationResult;

namespace ViaEventsAssociation.Core.Application.CommandHandler.Handlers.Event;

public class UpdateFromToHandler : ICommandHandler<UpdateFromToCommand>
{
    private readonly IVeaEventRepository _veaEventRepository;
    private readonly IUnitOfWork _unitOfWork;
    
    internal UpdateFromToHandler(IVeaEventRepository veaEventRepository, IUnitOfWork unitOfWork)
    {
        _veaEventRepository = veaEventRepository;
        _unitOfWork = unitOfWork;
    }
        
    public async Task<Result> HandleAsync(UpdateFromToCommand command)
    {
        var veaEventResult = await _veaEventRepository.FindAsync(command.EventId);
        if (veaEventResult.IsErrorResult())
        {
            return veaEventResult;
        }
        
        var result = veaEventResult.Value!.UpdateFromTo(command.FromTo);
        if (result.IsErrorResult())
        {
            return result;
        }
        
        await _unitOfWork.SaveChangesAsync();
        return result;
    }
}