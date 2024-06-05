using Microsoft.AspNetCore.Mvc;
using ViaEventAssociation.Core.QueryContracts.Queries;
using ViaEventAssociation.Core.QueryContracts.QueryDispatching;
using ViaEventAssociation.Presentation.WebAPI.Endpoints.Common;
using ViaEventsAssociation.Core.Tools.ObjectMapper;

namespace ViaEventAssociation.Presentation.WebAPI.Endpoints.Queries;

public class AvailableLocationsEndpoint(IMapper mapper, IQueryDispatcher dispatcher): ApiEndpoint.WithRequest<AvailableLocationsRequest>.WithResponse<AvailableLocationsResponse>
{
    [HttpPost("availableLocations/{Id}")]
    public override async Task<ActionResult<AvailableLocationsResponse>> HandleAsync(AvailableLocationsRequest request)
    {
        AvailableLocationsPage.Query query = mapper.Map<AvailableLocationsPage.Query>(request);
        AvailableLocationsPage.Answer answer = await dispatcher.DispatchAsync(query);
        AvailableLocationsResponse response = mapper.Map<AvailableLocationsResponse>(answer);
        return Ok(response);
    }
}

public record AvailableLocationsRequest([FromRoute] string Id);

public record AvailableLocationsResponse(List<AvailableLocationsPage.Location> Locations, AvailableLocationsPage.Event Events, string CreatorEmail);