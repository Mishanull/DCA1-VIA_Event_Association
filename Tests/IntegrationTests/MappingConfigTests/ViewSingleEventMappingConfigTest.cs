using ViaEventAssociation.Core.QueryContracts.Queries;
using ViaEventAssociation.Presentation.WebAPI.Endpoints.Queries;
using ViaEventAssociation.Presentation.WebAPI.MappingConfigs;

namespace IntegrationTests.MappingConfigTests;

public class MappingConfigsTests
{
    [Fact]
    public void ViewSingleEventRequestToQueryMappingConfig_MapsCorrectly()
    {
        // Arrange
        const string id = "1";
        const int pageNumber = 2;
        const int displayedRows = 10;
        const int rowSize = 20;
        var request = new ViewSingleEventRequest(id, pageNumber, displayedRows, rowSize);
        var mapper = new ViewSingleEventRequestToQueryMappingConfig();

        // Act
        var query = mapper.Map(request);

        // Assert
        Assert.Equal(request.Id, query.EventId);
        Assert.Equal(request.PageNumber, query.PageNumber);
        Assert.Equal(request.DisplayedRows, query.DisplayedRows);
        Assert.Equal(request.RowSize, query.RowSize);
    }

    [Fact]
    public void ViewSingleEventAnswerToResponseMappingConfig_MapsCorrectly()
    {
        // Arrange
        var answer = new SingleEventPage.Answer(
            new SingleEventPage.Event(
                "Event Title", "Description of the event", "Location Name", "2023-10-01", "2023-10-02", "Public", 100
            ),
            new List<SingleEventPage.Guest> {
                new SingleEventPage.Guest("GUEST123", "John Doe", "url/to/picture")
            },
            150
        );
        var mapper = new ViewSingleEventAnswerToResponseMappingConfig();

        // Act
        var response = mapper.Map(answer);

        // Assert
        Assert.NotNull(response.Event);
        Assert.Equal(answer.Event.Title, response.Event.Title);
        Assert.Equal(answer.Guest[0].FirstName, response.Guests[0].FirstName);
        Assert.Equal(answer.GuestsCount, response.GuestCount);
    }
}