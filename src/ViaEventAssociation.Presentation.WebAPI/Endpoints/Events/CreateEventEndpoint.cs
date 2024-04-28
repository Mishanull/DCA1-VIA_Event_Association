using Microsoft.AspNetCore.Mvc;
using ViaEventAssociation.Core.Domain.Contracts.UnitOfWork;
using ViaEventAssociation.Core.Domain.Services;
using ViaEventAssociation.Presentation.WebAPI.Endpoints.Common;
using ViaEventsAssociation.Core.Application.AppEntry;
using ViaEventsAssociation.Core.Application.CommandHandler.Commands.Event;

namespace ViaEventAssociation.Presentation.WebAPI.Endpoints.Events;

public class CreateEventEndpoint(ICommandDispatcher commandDispatcher, IUnitOfWork unitOfWork)
    : ApiEndpoint.WithRequest<CreateEventRequest>.WithoutResponse
{
    [HttpPost("events/create")]
    public override async Task<ActionResult> HandleAsync([FromBody]CreateEventRequest request)
    {
        var commandResult = CreateEventCommand.Create(request.CreatorId, new CurrentTime());
        if (commandResult.IsErrorResult())
        {
            return BadRequest(commandResult.Errors);
        }

        var saveDispatcher = new UowSaveDispatcher(commandDispatcher, unitOfWork);
        var dispatchResult = await saveDispatcher.Dispatch(commandResult.Value!);
        return !dispatchResult.IsErrorResult() ? Ok() : BadRequest(dispatchResult.Errors);
    }
}

public record CreateEventRequest([FromBody]string CreatorId);
