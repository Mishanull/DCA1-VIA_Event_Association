using Microsoft.AspNetCore.Mvc;
using ViaEventAssociation.Core.Domain.Contracts.UnitOfWork;
using ViaEventAssociation.Presentation.WebAPI.Endpoints.Common;
using ViaEventsAssociation.Core.Application.AppEntry;
using ViaEventsAssociation.Core.Application.CommandHandler.Commands.Event;

namespace ViaEventAssociation.Presentation.WebAPI.Endpoints.Events;

public class UpdateDescriptionEndpoint(ICommandDispatcher dispatcher, IUnitOfWork unitOfWork) : ApiEndpoint.WithRequest<UpdateDescriptionRequest>.WithoutResponse
{
    [HttpPost("events/updateDescription")]
    public override async Task<ActionResult> HandleAsync([FromBody] UpdateDescriptionRequest updateDescriptionRequest)
    {
        var commandResult = UpdateDescriptionCommand.Create(updateDescriptionRequest.EventId, updateDescriptionRequest.Description);
        if (commandResult.IsErrorResult())
        {
            return BadRequest(commandResult.Errors);
        }
        var saveDispatcher = new UowSaveDispatcher(dispatcher, unitOfWork);
        var dispatchResult = await saveDispatcher.Dispatch(commandResult.Value!);
        return !dispatchResult.IsErrorResult() ? Ok() : BadRequest(dispatchResult.Errors);
    }
}

public class UpdateDescriptionRequest
{
    public string EventId { get; set; }  
    public string Description { get; set; }   
}