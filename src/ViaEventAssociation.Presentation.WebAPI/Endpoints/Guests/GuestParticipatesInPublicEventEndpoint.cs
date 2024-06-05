using Microsoft.AspNetCore.Mvc;
using ViaEventAssociation.Core.Domain.Contracts.UnitOfWork;
using ViaEventAssociation.Presentation.WebAPI.Endpoints.Common;
using ViaEventsAssociation.Core.Application.AppEntry;
using ViaEventsAssociation.Core.Application.CommandHandler.Commands.Guest;

namespace ViaEventAssociation.Presentation.WebAPI.Endpoints.Guests;

public class GuestParticipatesInPublicEventEndpoint(ICommandDispatcher dispatcher, IUnitOfWork unitOfWork) : ApiEndpoint.WithRequest<GuestParticipatesInPublicEventRequest>.WithoutResponse
{
    [HttpPost("guest/participate-public-event")]
    public override async Task<ActionResult> HandleAsync(GuestParticipatesInPublicEventRequest request)
    {
        var commandResult =
            GuestParticipatesInPublicEventCommand.Create(request.EventId, request.GuestId, request.Reason);
        if (commandResult.IsErrorResult())
        {
            return BadRequest(commandResult.Errors);
        }

        var uowDispatcher = new UowSaveDispatcher(dispatcher, unitOfWork);
        var result = await uowDispatcher.Dispatch(commandResult.Value!);
        if (result.IsErrorResult())
        {
            return BadRequest(commandResult.Errors);
        }

        return Ok();
    }
}

public record GuestParticipatesInPublicEventRequest([FromBody] string EventId, [FromBody] string GuestId, [FromBody] string Reason);