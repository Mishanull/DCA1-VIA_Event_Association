using ViaEventAssociation.Core.QueryContracts.Queries;
using ViaEventAssociation.Presentation.WebAPI.Endpoints.Queries;
using ViaEventAssociation.Presentation.WebAPI.MappingConfigs;
using ViaEventsAssociation.Core.Tools.ObjectMapper;

namespace ViaEventAssociation.Presentation.WebAPI.ServiceRegistration;

public static class ServiceCollectionExtensions
{
    public static void RegisterMappingConfigs(this IServiceCollection services)
    {
        services
            .AddScoped<IMappingConfig<ViewSingleEventRequest, SingleEventPage.Query>,
                ViewSingleEventRequestToQueryMappingConfig>();
        services
            .AddScoped<IMappingConfig<SingleEventPage.Answer, ViewSingleEventResponse>,
                ViewSingleEventAnswerToResponseMappingConfig>();
        services
            .AddScoped<IMappingConfig<AvailableLocationsRequest, AvailableLocationsPage.Query>,
                AvailableLocationsRequestToQueryMappingConfig>();
        services.AddScoped<IMappingConfig<AvailableLocationsPage.Answer, AvailableLocationsResponse>,
            AvailableLocationsAnswerToResponseMappingConfig>();
    }
}