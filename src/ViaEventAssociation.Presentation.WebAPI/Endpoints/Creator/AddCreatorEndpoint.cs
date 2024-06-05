using Microsoft.AspNetCore.Mvc;
using UnitTests.FakeServices;
using ViaEventAssociation.Core.Domain.Contracts.UnitOfWork;
using ViaEventAssociation.Presentation.WebAPI.Endpoints.Common;
using ViaEventsAssociation.Core.Application.AppEntry;
using ViaEventsAssociation.Core.Application.CommandHandler.Commands.Creator;

namespace ViaEventAssociation.Presentation.WebAPI.Endpoints.Creator;

public class AddCreatorEndpoint (ICommandDispatcher dispatcher, IUnitOfWork unitOfWork) : ApiEndpoint.WithRequest<AddCreatorRequest>.WithoutResponse
{
    [HttpPost("creator/add")]
    public override async Task<ActionResult> HandleAsync(AddCreatorRequest request)
    {
        var commandResult = AddCreatorCommand.Create(request.email, new FakeEmailCheck());
        if (commandResult.IsErrorResult())
        {
            return BadRequest(commandResult.Errors);
        }

        var saveDispatcher = new UowSaveDispatcher(dispatcher, unitOfWork);
        var dispatchResult = await saveDispatcher.Dispatch(commandResult.Value!);
        return dispatchResult.IsErrorResult() ? BadRequest(dispatchResult.Errors) : Ok();
    }
}

public record AddCreatorRequest(string email);