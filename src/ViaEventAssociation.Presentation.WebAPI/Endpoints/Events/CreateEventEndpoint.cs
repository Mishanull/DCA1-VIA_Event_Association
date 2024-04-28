using Microsoft.AspNetCore.Mvc;
using ViaEventAssociation.Core.Domain.Services;
using ViaEventAssociation.Presentation.WebAPI.Endpoints.Common;
using ViaEventsAssociation.Core.Application.AppEntry;
using ViaEventsAssociation.Core.Application.CommandHandler.Commands.Event;

namespace ViaEventAssociation.Presentation.WebAPI.Endpoints.Events;

public class CreateEventEndpoint(ICommandDispatcher commandDispatcher)
    : ApiEndpoint.WithRequest<CreateEventRequest>.WithoutResponse
{
    [HttpPost("events/{Id}/create")]
    public override async Task<ActionResult> HandleAsync(CreateEventRequest request)
    {
        var commandResult = CreateEventCommand.Create(request.CreatorId, new CurrentTime());
        if (commandResult.IsErrorResult())
        {
            return BadRequest(commandResult.Errors);
        }

        var dispatchResult = await commandDispatcher.Dispatch(commandResult.Value!);
        return dispatchResult.IsErrorResult() ? Ok() : BadRequest(dispatchResult.Errors);
    }
}

public record CreateEventRequest([FromRoute]string CreatorId);
