using IntegrationTests.Utils;
using ViaEventAssociation.Core.QueryContracts.Queries;
using ViaEventAssociation.Infrastructure.EfcQueries.QueryHandlers;

namespace IntegrationTests.QueryTests;

public class SingleEventQueryTests
{
    [Fact]
    public async void FetchPageView()
    {
        //Arrange
        await using var ctx = DbContextHelper.SetupReadContext().Seed();
        //get event
        var singleEvent = await ctx.Events.FindAsync("27a5bde5-3900-4c45-9358-3d186ad6b2d7");
        var query = new SingleEventPage.Query(singleEvent.Id, 1, 2, 3);
        var handler = new SingleEventPageQueryHandler(ctx);

        //Act
        var answer = await handler.HandleAsync(query);
        
        //Assert
        Assert.NotNull(answer);
        Assert.NotNull(answer.Event);
        Assert.Equal(2, answer.GuestsCount);
    }
}