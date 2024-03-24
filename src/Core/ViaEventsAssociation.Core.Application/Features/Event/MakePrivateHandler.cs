﻿using ViaEventAssociation.Core.Domain.Contracts.Repositories;
using ViaEventAssociation.Core.Domain.Contracts.UnitOfWork;
using ViaEventsAssociation.Core.Application.CommandDispatching.Commands.Event;
using ViaEventsAssociation.Core.Application.CommandDispatching.Common.Base;
using VIAEventsAssociation.Core.Tools.OperationResult.OperationResult;

namespace ViaEventsAssociation.Core.Application.Features.Event;

public class MakePrivateHandler : ICommandHandler<MakePrivateCommand>
{
    private readonly IVeaEventRepository _veaEventRepository;
    private readonly IUnitOfWork _unitOfWork;
    
    internal MakePrivateHandler(IVeaEventRepository veaEventRepository, IUnitOfWork unitOfWork)
    {
        _veaEventRepository = veaEventRepository;
        _unitOfWork = unitOfWork;
    }
    
    public async Task<Result> HandleAsync(MakePrivateCommand command)
    {
        var veaEvent = await _veaEventRepository.FindAsync(command.EventId);
        var result = veaEvent.Value.MakePrivate();
        
        if (result.IsErrorResult())
        {
            return result;
        }
        
        await _unitOfWork.SaveChangesAsync();
        return result;
    }
    
}