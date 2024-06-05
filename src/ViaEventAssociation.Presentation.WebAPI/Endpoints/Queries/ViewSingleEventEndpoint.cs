using Microsoft.AspNetCore.Mvc;
using ViaEventAssociation.Core.QueryContracts.Queries;
using ViaEventAssociation.Core.QueryContracts.QueryDispatching;
using ViaEventAssociation.Presentation.WebAPI.Endpoints.Common;
using ViaEventsAssociation.Core.Tools.ObjectMapper;

namespace ViaEventAssociation.Presentation.WebAPI.Endpoints.Queries;

public class ViewSingleEventEndpoint (IMapper mapper, IQueryDispatcher dispatcher) : ApiEndpoint.WithRequest<ViewSingleEventRequest>.WithResponse<ViewSingleEventResponse>
{
    [HttpPost("events/{Id}")]
    public override async Task<ActionResult<ViewSingleEventResponse>> HandleAsync(ViewSingleEventRequest request)
    {
        SingleEventPage.Query query = mapper.Map<SingleEventPage.Query>(request);
        SingleEventPage.Answer answer = await dispatcher.DispatchAsync(query);
        ViewSingleEventResponse response = mapper.Map<ViewSingleEventResponse>(answer);
        return Ok(response);
    }
}

public record ViewSingleEventRequest([FromRoute]string Id, [FromBody]int PageNumber, [FromBody]int DisplayedRows, [FromBody]int RowSize);
public record ViewSingleEventResponse(SingleEventPage.Event Event, List<SingleEventPage.Guest> Guests, int GuestCount);