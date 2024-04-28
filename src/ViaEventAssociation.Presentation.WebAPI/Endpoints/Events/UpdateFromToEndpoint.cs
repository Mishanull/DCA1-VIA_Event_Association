using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using ViaEventAssociation.Core.Domain.Common.ValueObjects;
using ViaEventAssociation.Core.Domain.Contracts.UnitOfWork;
using ViaEventAssociation.Presentation.WebAPI.Endpoints.Common;
using ViaEventsAssociation.Core.Application.AppEntry;
using ViaEventsAssociation.Core.Application.CommandHandler.Commands.Event;

namespace ViaEventAssociation.Presentation.WebAPI.Endpoints.Events;

public class UpdateFromToEndpoint(ICommandDispatcher dispatcher, IUnitOfWork unitOfWork) : ApiEndpoint.WithRequest<UpdateFromToRequest>.WithoutResponse
{
    [HttpPost("events/updateFromTo")]
    public override async Task<ActionResult> HandleAsync([FromBody] UpdateFromToRequest updateFromToRequest)
    {
        var commandResult = UpdateFromToCommand.Create(updateFromToRequest.EventId, updateFromToRequest.From, updateFromToRequest.To);
        if (commandResult.IsErrorResult())
        {
            return BadRequest(commandResult.Errors);
        }
        var saveDispatcher = new UowSaveDispatcher(dispatcher, unitOfWork);
        var dispatchResult = await saveDispatcher.Dispatch(commandResult.Value!);
        return !dispatchResult.IsErrorResult() ? Ok() : BadRequest(dispatchResult.Errors);
    }
}

public class UpdateFromToRequest
{
    public string EventId { get; set; }  
    public DateTime From { get; set; }
    public DateTime To { get; set; }
}