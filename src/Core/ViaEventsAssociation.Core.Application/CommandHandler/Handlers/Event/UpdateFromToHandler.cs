using ViaEventAssociation.Core.Domain.Contracts;
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
    private readonly ICurrentTime _currentTime; 

    public UpdateFromToHandler(IVeaEventRepository veaEventRepository, IUnitOfWork unitOfWork, ICurrentTime currentTime)
    {
        _veaEventRepository = veaEventRepository;
        _unitOfWork = unitOfWork;
        _currentTime = currentTime;
    }
        
    public async Task<Result> HandleAsync(UpdateFromToCommand command)
    {
        var veaEventResult = await _veaEventRepository.FindAsync(command.EventId);
        if (veaEventResult.IsErrorResult())
        {
            return veaEventResult;
        }

        veaEventResult.Value!.CurrentTimeProvider = _currentTime;
        var result = veaEventResult.Value!.UpdateFromTo(command.FromTo);
        if (result.IsErrorResult())
        {
            return result;
        }
        
        await _unitOfWork.SaveChangesAsync();
        return result;
    }
}