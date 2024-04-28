using ObjectMapper;
using ViaEventAssociation.Core.QueryContracts.Queries;
using ViaEventAssociation.Presentation.WebAPI.Endpoints.Queries;

namespace ViaEventAssociation.Presentation.WebAPI.MappingConfigs;

public class AvailableLocationsRequestToQueryMappingConfig : IMappingConfig<AvailableLocationsRequest, AvailableLocationsPage.Query>
{
    public AvailableLocationsPage.Query Map(AvailableLocationsRequest input) => new(input.Id);
}

public class AvailableLocationsAnswerToResponseMappingConfig : IMappingConfig<AvailableLocationsPage.Answer, AvailableLocationsResponse>
{
    public AvailableLocationsResponse Map(AvailableLocationsPage.Answer input) =>
        new(input.Locations, input.Event, input.CreatorEmail);
}
