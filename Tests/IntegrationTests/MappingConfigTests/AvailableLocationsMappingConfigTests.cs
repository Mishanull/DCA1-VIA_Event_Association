using ViaEventAssociation.Presentation.WebAPI.Endpoints.Queries;
using ViaEventAssociation.Presentation.WebAPI.MappingConfigs;

namespace IntegrationTests.MappingConfigTests;

public class AvailableLocationsMappingConfigTests
{
    [Fact]
    public void AvailableLocationsRequestToQueryMappingConfig_MapsCorrectly()
    {
        // Arrange
        const string id = "123";
        var request = new AvailableLocationsRequest(id);
        var mapper = new AvailableLocationsRequestToQueryMappingConfig();

        // Act
        var query = mapper.Map(request);

        // Assert
        Assert.NotNull(query);
        Assert.Equal("123", query.EventId);
    }
}