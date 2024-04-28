using ViaEventAssociation.Core.QueryContracts.Queries;
using ViaEventAssociation.Presentation.WebAPI.Endpoints.Queries;
using ViaEventsAssociation.Core.Tools.ObjectMapper;

namespace ViaEventAssociation.Presentation.WebAPI.MappingConfigs;

public class ViewSingleEventRequestToQueryMappingConfig : IMappingConfig<ViewSingleEventRequest, SingleEventPage.Query>
{
    public SingleEventPage.Query Map(ViewSingleEventRequest input) =>
        new(input.Id, input.PageNumber, input.DisplayedRows, input.RowSize);
}

public class ViewSingleEventAnswerToResponseMappingConfig : IMappingConfig<SingleEventPage.Answer, ViewSingleEventResponse>
{
    public ViewSingleEventResponse Map(SingleEventPage.Answer input) =>
        new(input.Event, input.Guest, input.GuestsCount);
}