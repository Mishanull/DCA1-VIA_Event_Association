using Microsoft.AspNetCore.Mvc;
using ObjectMapper;
using ViaEventAssociation.Core.QueryContracts.Queries;
using ViaEventAssociation.Core.QueryContracts.QueryDispatching;
using ViaEventAssociation.Presentation.WebAPI.Endpoints.Common;

namespace ViaEventAssociation.Presentation.WebAPI.Endpoints.Queries;

public class ViewSingleEventEndpoint (IMapper mapper, IQueryDispatcher dispatcher) : ApiEndpoint.WithRequest<ViewSingleEventRequest>.WithResponse<ViewSingleEventResponse>
{
    [HttpGet("events/{Id}")]
    public override async Task<ActionResult<ViewSingleEventResponse>> HandleAsync(ViewSingleEventRequest request)
    {
        SingleEventPage.Query query = mapper.Map<SingleEventPage.Query>(request);
        SingleEventPage.Answer answer = await dispatcher.DispatchAsync(query);
        ViewSingleEventResponse response = mapper.Map<ViewSingleEventResponse>(answer);
        return Ok(response);
    }
}

public record ViewSingleEventRequest([FromRoute]string Id, [FromQuery]int PageNumber, [FromQuery]int DisplayedRows, [FromQuery]int RowSize);
public record ViewSingleEventResponse(SingleEventPage.Event Event, List<SingleEventPage.Guest> Guests, int GuestCount);