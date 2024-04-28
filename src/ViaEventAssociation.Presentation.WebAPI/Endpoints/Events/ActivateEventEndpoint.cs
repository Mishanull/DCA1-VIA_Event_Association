using Microsoft.AspNetCore.Mvc;
using ViaEventAssociation.Presentation.WebAPI.Endpoints.Common;
using ViaEventsAssociation.Core.Application.AppEntry;
using ViaEventsAssociation.Core.Application.CommandHandler.Commands.Event;

namespace ViaEventAssociation.Presentation.WebAPI.Endpoints.Events;

public class ActivateEventEndpoint(ICommandDispatcher dispatcher) : ApiEndpoint.WithRequest<ActivateEventRequest>.WithoutResponse
{
    [HttpPost("events/{Id}/activate")]
    public override async Task<ActionResult> HandleAsync(ActivateEventRequest activateEventRequest)
    {
        var commandResult = ActivateEventCommand.Create(activateEventRequest.Id);
        if (commandResult.IsErrorResult())
        {
            return BadRequest(commandResult.Errors);
        }

        var dispatchResult = await dispatcher.Dispatch(commandResult.Value!);
        return dispatchResult.IsErrorResult() ? Ok() : BadRequest(dispatchResult.Errors);
    }
}

public record ActivateEventRequest([FromRoute] string Id);
