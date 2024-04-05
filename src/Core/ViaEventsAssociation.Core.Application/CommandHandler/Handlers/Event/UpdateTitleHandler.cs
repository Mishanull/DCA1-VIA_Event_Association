using ViaEventAssociation.Core.Domain.Contracts.Repositories;
using ViaEventAssociation.Core.Domain.Contracts.UnitOfWork;
using ViaEventsAssociation.Core.Application.CommandHandler.Commands.Event;
using ViaEventsAssociation.Core.Application.CommandHandler.Common.Base;
using VIAEventsAssociation.Core.Tools.OperationResult.OperationResult;

namespace ViaEventsAssociation.Core.Application.CommandHandler.Handlers.Event;

public class UpdateTitleHandler : ICommandHandler<UpdateTitleCommand>
{
    private readonly IVeaEventRepository _veaEventRepository;
    private readonly IUnitOfWork _unitOfWork;
    
    internal UpdateTitleHandler(IVeaEventRepository veaEventRepository, IUnitOfWork unitOfWork)
    {
        _veaEventRepository = veaEventRepository;
        _unitOfWork = unitOfWork;
    }
    
    public async Task<Result> HandleAsync(UpdateTitleCommand command)
    {
        var veaEventResult = await _veaEventRepository.FindAsync(command.EventId);
        if (veaEventResult.IsErrorResult())
        {
            return veaEventResult;
        }
        
        var result = veaEventResult.Value!.UpdateTitle(command.Title);
        if (result.IsErrorResult())
        {
            return result;
        }
        
        await _unitOfWork.SaveChangesAsync();
        return result;
    }
}