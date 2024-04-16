using IntegrationTests.Utils;
using UnitTests.FakeServices;
using ViaEventAssociation.Core.QueryContracts.Queries;
using ViaEventAssociation.Infrastructure.EfcQueries.QueryHandlers;

namespace IntegrationTests.QueryTests;

public class UpcomingEventsPageQueryTests
{
    [Fact]
    public async void FetchUpcomingEvents()
    {
        //Arrange
        await using var ctx = DbContextHelper.SetupReadContext().Seed();
        var query = new UpcomingEventsPage.Query(4,9,"");
        var handler = new UpcomingEventsPageQueryHandler(ctx, new FakeCurrentTime(new DateTime(2024,1,14,17,0,0)));

        //Act
        var answer = await handler.HandleAsync(query);
        
        //Assert
        Assert.NotNull(answer);
        Assert.NotNull(answer.Events);
    }
}