using Microsoft.AspNetCore.Mvc;
using ViaEventAssociation.Core.Domain.Contracts.UnitOfWork;
using ViaEventAssociation.Presentation.WebAPI.Endpoints.Common;
using ViaEventsAssociation.Core.Application.AppEntry;
using ViaEventsAssociation.Core.Application.CommandHandler.Commands.Event;

namespace ViaEventAssociation.Presentation.WebAPI.Endpoints.Events;

public class UpdateTitleEndpoint(ICommandDispatcher dispatcher, IUnitOfWork unitOfWork) : ApiEndpoint.WithRequest<UpdateTitleRequest>.WithoutResponse
{
    [HttpPost("events/updateTitle")]
    public override async Task<ActionResult> HandleAsync([FromBody] UpdateTitleRequest updateTitleRequest)
    {
        var commandResult = UpdateTitleCommand.Create(updateTitleRequest.EventId, updateTitleRequest.Title);
        if (commandResult.IsErrorResult())
        {
            return BadRequest(commandResult.Errors);
        }
        var saveDispatcher = new UowSaveDispatcher(dispatcher, unitOfWork);
        var dispatchResult = await saveDispatcher.Dispatch(commandResult.Value!);
        return !dispatchResult.IsErrorResult() ? Ok() : BadRequest(dispatchResult.Errors);
    }
}

public class UpdateTitleRequest
{
    public string EventId { get; set; }  
    public string Title { get; set; }   
}